function getThemeColors() {
    const rootStyle = getComputedStyle(document.documentElement);
    const textColor = rootStyle.getPropertyValue('--mud-palette-text-primary').trim();
    const gridColor = rootStyle.getPropertyValue('--mud-palette-lines-default').trim();

    return { textColor, gridColor };
}

// Generic function to render any chart with configuration
export function renderChart(canvasId, chartConfig) {
    const canvas = document.getElementById(canvasId);
    if (!canvas) {
        console.error(`Canvas with ID ${canvasId} not found`);
        return;
    }

    // Destroy existing chart if it exists
    Chart.getChart(canvas)?.destroy();

    const { textColor, gridColor } = getThemeColors();

    // Apply theme colors to chart configuration
    const config = {
        type: chartConfig.type || 'line',
        data: {
            labels: chartConfig.labels || [],
            datasets: chartConfig.datasets || []
        },
        options: {
            responsive: chartConfig.options?.responsive ?? true,
            maintainAspectRatio: chartConfig.options?.maintainAspectRatio ?? false,
            plugins: {
                legend: {
                    display: chartConfig.options?.legend?.display ?? true,
                    position: chartConfig.options?.legend?.position || 'top',
                    labels: {
                        color: textColor
                    }
                }
            }
        }
    };

    // Add scales configuration for charts that support it
    if (['line', 'bar', 'scatter'].includes(config.type)) {
        config.options.scales = {
            y: {
                ticks: { color: textColor },
                grid: { color: gridColor }
            },
            x: {
                ticks: { color: textColor },
                grid: { color: gridColor }
            }
        };
    }

    // Apply any custom options
    if (chartConfig.options?.scales) {
        config.options.scales = {
            ...config.options.scales,
            ...chartConfig.options.scales
        };
    }

    new Chart(canvas, config);
}

// Render chart by card ID
export function renderChartByCardId(cardId, chartConfig) {
    const canvasId = `${cardId}Canvas`;
    renderChart(canvasId, chartConfig);
}