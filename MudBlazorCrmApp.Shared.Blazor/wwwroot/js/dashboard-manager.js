let grid = null;
let dotNetHelper = null;

function populateGridContent() {
    if (!grid) return;

    const gridItems = grid.getGridItems();
    console.log(`JS: Populating content for ${gridItems.length} items.`);

    gridItems.forEach(itemElement => {
        const widgetId = itemElement.getAttribute('gs-id');
        if (!widgetId) {
            console.error("JS: Found a grid item without a gs-id!", itemElement);
            return;
        }

        const contentPlaceholder = document.getElementById(`content-for-${widgetId}`);
        const contentHost = itemElement.querySelector('.grid-stack-item-content');

        if (contentPlaceholder && contentHost) {
            // Only populate if the content host is empty
            if (contentHost.children.length === 0) {
                // Clone the content instead of moving it
                const clonedContent = contentPlaceholder.cloneNode(true);
                // Move all children from the cloned placeholder to the visible card
                while (clonedContent.firstChild) {
                    contentHost.appendChild(clonedContent.firstChild);
                }
            }
        } else {
            console.error(`JS: Failed to populate content for ID ${widgetId}.`);
        }
    });

    // After populating content, ensure drag handles are properly configured
    configureDragHandles();
}

function configureDragHandles() {
    // Make sure only the move handle can be used for dragging
    const gridItems = document.querySelectorAll('.grid-stack-item');
    gridItems.forEach(item => {
        // Disable dragging on the entire item
        item.style.cursor = 'default';

        // Find the move handle and configure it properly
        const moveHandle = item.querySelector('.move-handle');
        if (moveHandle) {
            moveHandle.style.cursor = 'move';
            moveHandle.setAttribute('draggable', 'false'); // Prevent HTML5 drag

            // Ensure the move handle is properly positioned for dragging
            moveHandle.style.position = 'relative';
            moveHandle.style.zIndex = '10';
        }

        // Prevent dragging from other parts of the card
        const cardBody = item.querySelector('.card-body');
        if (cardBody) {
            cardBody.style.cursor = 'default';
            cardBody.addEventListener('mousedown', preventDragFromCardBody);
        }

        // Prevent dragging from card header (except move handle)
        const cardHeader = item.querySelector('.card-header');
        if (cardHeader) {
            cardHeader.addEventListener('mousedown', preventDragFromCardHeader);
        }
    });
}

function preventDragFromCardBody(event) {
    // If the click is not on the move handle, prevent dragging
    if (!event.target.closest('.move-handle')) {
        event.preventDefault();
        event.stopPropagation();
    }
}

function preventDragFromCardHeader(event) {
    // If the click is not on the move handle, prevent dragging
    if (!event.target.closest('.move-handle')) {
        event.preventDefault();
        event.stopPropagation();
    }
}

// Main initialization function - Fixed to properly target the grid container
export function init(containerSelector, dotNetHelperRef, layoutData) {
    console.log("JS: Initializing grid from DATA.", layoutData);

    dotNetHelper = dotNetHelperRef;

    // Find the dashboard container (the one with overflow and scrolling)
    const dashboardContainer = document.querySelector('.dashboard-container');
    if (!dashboardContainer) {
        console.error('JS: Dashboard container not found');
        return;
    }

    // Find the grid-stack element inside the dashboard container
    const gridStackElement = dashboardContainer.querySelector('.grid-stack');
    if (!gridStackElement) {
        console.error('JS: Grid stack element not found');
        return;
    }

    // Initialize GridStack with proper configuration
    grid = GridStack.init({
        float: true,
        cellHeight: '70px',
        handle: '.move-handle', // Only allow dragging from move handle
        disableOneColumnMode: true, // Prevent layout changes in small screens
        dragIn: false, // Disable dragging in from outside
        dragOut: false, // Disable dragging out
        removable: false, // Disable removing by dragging out
        margin: 10, // Add some margin between cards
        animate: true, // Smooth animations
        draggable: {
            handle: '.move-handle',
            scroll: false, // Prevent scroll during drag
            appendTo: 'parent' // Keep drag within parent container
        },
        resizable: {
            handles: 'e, se, s, sw, w' // Allow resizing from specific handles
        }
    }, gridStackElement); // Target the grid-stack element directly

    // load() creates the widgets from the data array
    grid.load(layoutData);

    // After loading, populate the content
    requestAnimationFrame(() => {
        populateGridContent();
        configureDragHandles();
    });

    grid.on('change', (event, items) => {
        if (!items) return; 
        const newLayout = items.map(item => ({
            id: item.id,
            x: item.x, y: item.y, w: item.w, h: item.h
        }));
        dotNetHelper.invokeMethodAsync('OnLayoutUpdatedFromJS', newLayout);
    });

    // Add drag start event to ensure proper cursor positioning
    grid.on('dragstart', (event, el) => {
        // Ensure the cursor stays properly positioned
        const moveHandle = el.querySelector('.move-handle');
        if (moveHandle) {
            moveHandle.style.cursor = 'grabbing';
        }
    });

    // Add drag stop event to restore cursor
    grid.on('dragstop', (event, el) => {
        const moveHandle = el.querySelector('.move-handle');
        if (moveHandle) {
            moveHandle.style.cursor = 'move';
        }
    });
}

// Function to set up event delegation for delete buttons
window.setupDeleteEventDelegation = function (dotNetHelperRef) {
    dotNetHelper = dotNetHelperRef;

    // Remove existing event listener if any
    document.removeEventListener('click', handleDeleteClick);

    // Add event listener for delete buttons
    document.addEventListener('click', handleDeleteClick);
};

function handleDeleteClick(event) {
    const deleteHandle = event.target.closest('.delete-handle');
    if (deleteHandle && dotNetHelper) {
        const cardId = deleteHandle.getAttribute('data-card-id');
        if (cardId) {
            dotNetHelper.invokeMethodAsync('HandleDeleteCard', cardId);
        }
    }
}

// Function to add a single new card instead of reloading everything
export function addCard(cardData) {
    if (grid) {
        // Ensure the new card is added within the grid bounds
        //const gridBounds = grid.getGridHeight();
        const maxWidth = grid.getColumn();

        // Clamp the card position to stay within bounds
        if (cardData.x + cardData.w > maxWidth) {
            cardData.x = Math.max(0, maxWidth - cardData.w);
        }

        grid.addWidget(cardData);

        requestAnimationFrame(() => {
            populateGridContent();
            configureDragHandles();
        });
    }
}

// Function to completely clear and reload the grid with new data
export function loadData(layoutData) {
    if (grid) {
        grid.load(layoutData);
        requestAnimationFrame(() => {
            populateGridContent();
            configureDragHandles();
        });
    }
}

export function destroy() {
    if (grid) {
        grid.destroy();
        grid = null;
    }
    // Clean up event listener
    document.removeEventListener('click', handleDeleteClick);
    dotNetHelper = null;
}