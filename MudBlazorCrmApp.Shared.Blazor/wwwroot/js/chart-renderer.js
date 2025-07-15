export function renderSalesChart() {
    const canvas = document.getElementById('salesChartCanvas');
    if (!canvas) return;
    Chart.getChart(canvas)?.destroy();
    new Chart(canvas, { type: 'line', data: { labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'], datasets: [{ label: 'Sales ($k)', data: [12, 19, 9, 17, 22, 15], borderColor: '#007bff', tension: 0.3 }] }, options: { responsive: true, maintainAspectRatio: false } });
}

export function renderLeadSourceChart() {
    const canvas = document.getElementById('leadSourceChartCanvas');
    if (!canvas) return;
    Chart.getChart(canvas)?.destroy();
    new Chart(canvas, { type: 'doughnut', data: { labels: ['Organic', 'Referral', 'Paid Ads'], datasets: [{ data: [300, 50, 100], backgroundColor: ['#28a745', '#ffc107', '#dc3545'] }] }, options: { responsive: true, maintainAspectRatio: false } });
}