// Глобальные переменные для хранения ссылок на графики
let yieldChartInstance = null;
let areaChartInstance = null;
let comparisonChartInstance = null;

// Функция для уничтожения конкретного графика
function destroyChart(chartInstance) {
    if (chartInstance) {
        chartInstance.destroy();
        return null;
    }
    return null;
}

// Функция для уничтожения всех графиков
function destroyAllCharts() {
    console.log('Уничтожение всех графиков...');
    yieldChartInstance = destroyChart(yieldChartInstance);
    areaChartInstance = destroyChart(areaChartInstance);
    comparisonChartInstance = destroyChart(comparisonChartInstance);
}

// Функция для создания графика урожайности
function createYieldChart(data) {
    const ctx = document.getElementById('yieldChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.labels,
            datasets: [{
                label: 'Урожайность (ц/га)',
                data: data.values,
                borderColor: '#2E7D32',
                backgroundColor: 'rgba(46, 125, 50, 0.1)',
                tension: 0.1,
                fill: true
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Динамика урожайности',
                    color: '#2E7D32'
                }
            }
        }
    });
}

// Функция для создания графика посевных площадей
function createAreaChart(data) {
    const ctx = document.getElementById('areaChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'pie',
        data: {
            labels: data.labels,
            datasets: [{
                data: data.values,
                backgroundColor: [
                    '#2E7D32',
                    '#4CAF50',
                    '#66BB6A',
                    '#81C784',
                    '#A5D6A7'
                ]
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Распределение посевных площадей',
                    color: '#2E7D32'
                }
            }
        }
    });
}

// Функция для создания графика сравнения регионов
function createComparisonChart(data) {
    const ctx = document.getElementById('comparisonChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: data.labels,
            datasets: [{
                label: 'Урожайность (ц/га)',
                data: data.yield,
                backgroundColor: 'rgba(46, 125, 50, 0.5)'
            }, {
                label: 'Посевная площадь (тыс. га)',
                data: data.area,
                backgroundColor: 'rgba(76, 175, 80, 0.5)'
            }, {
                label: 'Валовый сбор (тыс. т)',
                data: data.harvest,
                backgroundColor: 'rgba(102, 187, 106, 0.5)'
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Сравнение регионов',
                    color: '#2E7D32'
                }
            },
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}

// Функция для обновления всех графиков
function updateCharts(data) {
    createYieldChart(data.yield);
    createAreaChart(data.area);
    createComparisonChart(data.comparison);
}

// Функция для создания графика урожайности по годам
function createYieldByYearChart(data) {
    const ctx = document.getElementById('yieldChart').getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.map(item => item.year),
            datasets: [{
                label: 'Урожайность (ц/га)',
                data: data.map(item => item.yield),
                borderColor: '#2E7D32',
                backgroundColor: 'rgba(46, 125, 50, 0.1)',
                tension: 0.1,
                fill: true
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Динамика урожайности зерновых культур',
                    color: '#2E7D32'
                }
            }
        }
    });
}

// Функция для создания графика урожайности по областям
function createYieldByRegionChart(data) {
    const ctx = document.getElementById('regionChart').getContext('2d');
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: data.map(item => item.region),
            datasets: [{
                label: 'Урожайность (ц/га)',
                data: data.map(item => item.yield),
                backgroundColor: 'rgba(46, 125, 50, 0.5)',
                borderColor: '#2E7D32',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Урожайность зерновых по областям',
                    color: '#2E7D32'
                }
            }
        }
    });
}

// Функция для создания графика посевных площадей
function createSownAreaChart(data) {
    const ctx = document.getElementById('sownAreaChart').getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.map(item => item.year),
            datasets: [{
                label: 'Посевная площадь (тыс. га)',
                data: data.map(item => item.sownArea),
                borderColor: '#4CAF50',
                backgroundColor: 'rgba(76, 175, 80, 0.1)',
                tension: 0.1,
                fill: true
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Динамика посевных площадей',
                    color: '#2E7D32'
                }
            }
        }
    });
}

// Функция для создания графика валового сбора
function createGrossHarvestChart(data) {
    const ctx = document.getElementById('grossHarvestChart').getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.map(item => item.year),
            datasets: [{
                label: 'Валовый сбор (тыс. тонн)',
                data: data.map(item => item.grossHarvest),
                borderColor: '#66BB6A',
                backgroundColor: 'rgba(102, 187, 106, 0.1)',
                tension: 0.1,
                fill: true
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Динамика валового сбора',
                    color: '#2E7D32'
                }
            }
        }
    });
}

// Функция для инициализации графиков
function initializeCharts(years, yields, areas) {
    console.log('Инициализация графиков...', { years, yields, areas });
    
    // Проверяем наличие элементов canvas
    const yieldChart = document.getElementById('yieldChart');
    const areaChart = document.getElementById('areaChart');

    if (!yieldChart || !areaChart) {
        console.error('Не найдены элементы canvas для графиков');
        return;
    }

    try {
        // Уничтожаем существующие графики
        yieldChartInstance = destroyChart(yieldChartInstance);
        areaChartInstance = destroyChart(areaChartInstance);

        // График урожайности
        const yieldCtx = yieldChart.getContext('2d');
        yieldChartInstance = new Chart(yieldCtx, {
            type: 'line',
            data: {
                labels: years,
                datasets: [{
                    label: 'Урожайность (ц/га)',
                    data: yields,
                    borderColor: '#6BAF92',
                    backgroundColor: 'rgba(107, 175, 146, 0.1)',
                    tension: 0.4,
                    fill: true
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'top',
                    },
                    title: {
                        display: true,
                        text: 'Динамика урожайности по годам'
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'ц/га'
                        }
                    }
                }
            }
        });

        // График площадей
        const areaCtx = areaChart.getContext('2d');
        areaChartInstance = new Chart(areaCtx, {
            type: 'bar',
            data: {
                labels: years,
                datasets: [{
                    label: 'Площадь (тыс. га)',
                    data: areas,
                    backgroundColor: '#DDB67D',
                    borderColor: '#7C5A41',
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'top',
                    },
                    title: {
                        display: true,
                        text: 'Распределение посевных площадей'
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'тыс. га'
                        }
                    }
                }
            }
        });

        console.log('Графики успешно инициализированы');
    } catch (error) {
        console.error('Ошибка при создании графиков:', error);
    }
}

function initializeComparisonChart(regions, period1Yields, period2Yields, period1Label, period2Label) {
    console.log('Инициализация графика сравнения...', { regions, period1Yields, period2Yields, period1Label, period2Label });
    
    const comparisonChart = document.getElementById('comparisonChart');
    if (!comparisonChart) {
        console.error('Не найден элемент canvas для графика сравнения');
        return;
    }

    try {
        // Уничтожаем существующий график сравнения
        comparisonChartInstance = destroyChart(comparisonChartInstance);

        const ctx = comparisonChart.getContext('2d');
        comparisonChartInstance = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: regions,
                datasets: [
                    {
                        label: period1Label,
                        data: period1Yields,
                        backgroundColor: 'rgba(107, 175, 146, 0.7)',
                        borderColor: '#6BAF92',
                        borderWidth: 1
                    },
                    {
                        label: period2Label,
                        data: period2Yields,
                        backgroundColor: 'rgba(221, 182, 125, 0.7)',
                        borderColor: '#DDB67D',
                        borderWidth: 1
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'top',
                    },
                    title: {
                        display: true,
                        text: 'Сравнение урожайности по регионам'
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'ц/га'
                        }
                    }
                }
            }
        });

        console.log('График сравнения успешно инициализирован');
    } catch (error) {
        console.error('Ошибка при создании графика сравнения:', error);
    }
}

// График для панели администратора: экспорты и отчёты по дням
function createAdminStatsChart(data) {
    const ctx = document.getElementById('adminStatsChart');
    if (!ctx) return;
    const chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.labels,
            datasets: [
                {
                    label: 'Экспортов',
                    data: data.exports,
                    borderColor: '#4e8c77',
                    backgroundColor: 'rgba(78,140,119,0.1)',
                    tension: 0.2,
                    fill: true
                },
                {
                    label: 'Отчётов',
                    data: data.reports,
                    borderColor: '#e57373',
                    backgroundColor: 'rgba(229,115,115,0.1)',
                    tension: 0.2,
                    fill: true
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: 'Динамика экспортов и отчётов по дням',
                    color: '#2d4c3c'
                }
            },
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
} 