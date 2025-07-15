function getThemeColors() {
    const rootStyle = getComputedStyle(document.documentElement);
    const textColor = rootStyle.getPropertyValue('--mud-palette-text-primary').trim();
    const gridColor = rootStyle.getPropertyValue('--mud-palette-lines-default').trim();

    return { textColor, gridColor };
}

export function renderSalesChart() {
    const canvas = document.getElementById('salesChartCanvas');
    if (!canvas) return;

    Chart.getChart(canvas)?.destroy();

    const { textColor, gridColor } = getThemeColors();

    new Chart(canvas, {
        type: 'line',
        data: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
            datasets: [{
                label: 'Sales ($k)',
                data: [12, 19, 9, 17, 22, 15],
                borderColor: '#007bff',
                tension: 0.3
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    ticks: { color: textColor },
                    grid: { color: gridColor }
                },
                x: {
                    ticks: { color: textColor },
                    grid: { color: gridColor }
                }
            },
            plugins: {
                legend: {
                    labels: { color: textColor }
                }
            }
        }
    });
}

export function renderLeadSourceChart() {
    const canvas = document.getElementById('leadSourceChartCanvas');
    if (!canvas) return;

    Chart.getChart(canvas)?.destroy();

    const { textColor } = getThemeColors();

    new Chart(canvas, {
        type: 'doughnut',
        data: {
            labels: ['Organic', 'Referral', 'Paid Ads'],
            datasets: [{
                data: [300, 50, 100],
                backgroundColor: ['#28a745', '#ffc107', '#dc3545']
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
                plugins: {
                legend: {
                    position: 'top',
                    labels: {
                        color: textColor
                    }
                }
            }
        }
    });
}