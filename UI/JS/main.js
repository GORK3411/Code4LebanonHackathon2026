// ========================================
// Ministry of Technology & AI Dashboard
// JavaScript - State Management & Charts
// ========================================

// ========================================
// MOCK DATA (Replace with API calls later)
// ========================================

const mockData = {
    // Dashboard - Service Requests
    serviceRequests: {
        labels: ['Week 1', 'Week 2', 'Week 3', 'Week 4', 'Week 5', 'Week 6', 'Week 7', 'Week 8'],
        datasets: [{
            label: 'Service Requests',
            data: [1200, 1900, 1500, 2200, 2800, 2400, 3100, 3500],
            borderColor: '#00703C',
            backgroundColor: 'rgba(0, 112, 60, 0.1)',
            borderWidth: 3,
            tension: 0.4,
            fill: true,
            pointBackgroundColor: '#ED1C24',
            pointRadius: 6,
            pointHoverRadius: 8
        }]
    },

    // National Coverage - Registration by Channels
    registration: {
        labels: ['NGOs', 'Universities', 'Employers', 'Public Sector', 'Syndicates'],
        datasets: [{
            data: [3245, 4892, 2156, 1789, 1465],
            backgroundColor: ['#06b6d4', '#3b82f6', '#a855f7', '#ef4444', '#f59e0b'],
            borderWidth: 2,
            borderColor: '#ffffff'
        }]
    },

    // Growth Metrics Data
    growthMetrics: {
        all: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [
                {
                    label: 'NGOs',
                    data: [1200, 1400, 1600, 1800, 2100, 2400, 2800, 3200, 3600, 4000, 4500, 5100],
                    borderColor: '#06b6d4',
                    backgroundColor: 'rgba(6, 182, 212, 0.1)',
                    borderWidth: 2,
                    tension: 0.4
                },
                {
                    label: 'Universities',
                    data: [1500, 1800, 2100, 2500, 2900, 3400, 3900, 4500, 5100, 5800, 6500, 7300],
                    borderColor: '#3b82f6',
                    backgroundColor: 'rgba(59, 130, 246, 0.1)',
                    borderWidth: 2,
                    tension: 0.4
                },
                {
                    label: 'Employers',
                    data: [800, 950, 1100, 1300, 1550, 1800, 2100, 2450, 2800, 3200, 3650, 4100],
                    borderColor: '#a855f7',
                    backgroundColor: 'rgba(168, 85, 247, 0.1)',
                    borderWidth: 2,
                    tension: 0.4
                },
                {
                    label: 'Public Sector',
                    data: [600, 750, 900, 1100, 1300, 1550, 1850, 2150, 2500, 2900, 3350, 3900],
                    borderColor: '#ef4444',
                    backgroundColor: 'rgba(239, 68, 68, 0.1)',
                    borderWidth: 2,
                    tension: 0.4
                },
                {
                    label: 'Syndicates',
                    data: [400, 550, 700, 850, 1050, 1300, 1600, 1950, 2350, 2800, 3300, 3900],
                    borderColor: '#f59e0b',
                    backgroundColor: 'rgba(245, 158, 11, 0.1)',
                    borderWidth: 2,
                    tension: 0.4
                }
            ]
        },
        ngo: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [{
                label: 'NGOs',
                data: [1200, 1400, 1600, 1800, 2100, 2400, 2800, 3200, 3600, 4000, 4500, 5100],
                borderColor: '#06b6d4',
                backgroundColor: 'rgba(6, 182, 212, 0.1)',
                borderWidth: 2,
                tension: 0.4
            }]
        },
        university: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [{
                label: 'Universities',
                data: [1500, 1800, 2100, 2500, 2900, 3400, 3900, 4500, 5100, 5800, 6500, 7300],
                borderColor: '#3b82f6',
                backgroundColor: 'rgba(59, 130, 246, 0.1)',
                borderWidth: 2,
                tension: 0.4
            }]
        },
        employer: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [{
                label: 'Employers',
                data: [800, 950, 1100, 1300, 1550, 1800, 2100, 2450, 2800, 3200, 3650, 4100],
                borderColor: '#a855f7',
                backgroundColor: 'rgba(168, 85, 247, 0.1)',
                borderWidth: 2,
                tension: 0.4
            }]
        },
        public: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [{
                label: 'Public Sector',
                data: [600, 750, 900, 1100, 1300, 1550, 1850, 2150, 2500, 2900, 3350, 3900],
                borderColor: '#ef4444',
                backgroundColor: 'rgba(239, 68, 68, 0.1)',
                borderWidth: 2,
                tension: 0.4
            }]
        },
        syndicate: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [{
                label: 'Syndicates',
                data: [400, 550, 700, 850, 1050, 1300, 1600, 1950, 2350, 2800, 3300, 3900],
                borderColor: '#f59e0b',
                backgroundColor: 'rgba(245, 158, 11, 0.1)',
                borderWidth: 2,
                tension: 0.4
            }]
        }
    },

    // Insights - Areas of Interest
    interests: {
        labels: ['Machine Learning', 'Deep Learning', 'NLP', 'Computer Vision', 'AI Ethics', 'Robotics', 'Data Science'],
        datasets: [{
            data: [2800, 3200, 1900, 2100, 1400, 1100, 2000],
            backgroundColor: ['#3b82f6', '#06b6d4', '#a855f7', '#ef4444', '#f59e0b', '#10b981', '#ec4899'],
            borderWidth: 2,
            borderColor: '#ffffff'
        }]
    },

    // Insights - Motivations
    motivations: {
        labels: ['Career Advancement', 'Skill Development', 'Job Security', 'Salary Increase', 'Personal Interest', 'Industry Requirements'],
        datasets: [{
            label: 'Number of Learners',
            data: [4500, 3800, 2900, 2600, 2100, 1500],
            backgroundColor: ['#00703C', '#ED1C24', '#3b82f6', '#f59e0b', '#10b981', '#a855f7'],
            borderWidth: 0
        }]
    },

    // Insights - Challenges
    challenges: {
        labels: ['Time Management', 'Technical Complexity', 'Lack of Mentorship', 'Cost Barriers', 'Language Barriers', 'Network Issues'],
        datasets: [{
            label: 'Learners Affected',
            data: [3400, 2800, 2200, 1900, 1600, 1200],
            backgroundColor: ['#ef4444', '#f97316', '#f59e0b', '#eab308', '#84cc16', '#22c55e'],
            borderWidth: 0
        }]
    },

    // Geographic Coverage - Regional Distribution
    regional: {
        labels: ['North Region', 'South Region', 'East Region', 'West Region'],
        datasets: [{
            label: 'Number of Learners',
            data: [2145, 1523, 3892, 3997],
            backgroundColor: ['#ef4444', '#f97316', '#10b981', '#3b82f6'],
            borderWidth: 0
        }]
    },

    // Geographic Coverage - Channel Effectiveness
    channelEffectiveness: {
        labels: ['Q1', 'Q2', 'Q3', 'Q4'],
        datasets: [
            {
                label: 'North',
                data: [500, 620, 750, 890],
                backgroundColor: '#ef4444'
            },
            {
                label: 'South',
                data: [380, 420, 510, 615],
                backgroundColor: '#f97316'
            },
            {
                label: 'East',
                data: [750, 920, 1100, 1280],
                backgroundColor: '#10b981'
            },
            {
                label: 'West',
                data: [800, 950, 1150, 1350],
                backgroundColor: '#3b82f6'
            }
        ]
    },

    // Learner Profile - Demographics
    demographics: {
        age: {
            labels: ['18-25', '26-35', '36-45', '46-55', '55+'],
            datasets: [{
                data: [4200, 5800, 2100, 1200, 600],
                backgroundColor: ['#06b6d4', '#3b82f6', '#a855f7', '#ef4444', '#f59e0b']
            }]
        },
        employment: {
            labels: ['Employed', 'Self-Employed', 'Unemployed', 'Student'],
            datasets: [{
                data: [8500, 2100, 1800, 1500],
                backgroundColor: ['#10b981', '#3b82f6', '#ef4444', '#f59e0b']
            }]
        },
        'job-level': {
            labels: ['Entry Level', 'Mid-Level', 'Senior', 'Manager', 'Executive'],
            datasets: [{
                data: [3200, 4100, 3400, 2100, 1100],
                backgroundColor: ['#06b6d4', '#3b82f6', '#a855f7', '#f59e0b', '#ef4444']
            }]
        },
        industry: {
            labels: ['Finance', 'Healthcare', 'Tech', 'Education', 'Government', 'Retail', 'Other'],
            datasets: [{
                data: [2100, 1900, 4500, 2300, 1800, 800, 900],
                backgroundColor: ['#3b82f6', '#ef4444', '#10b981', '#f59e0b', '#a855f7', '#06b6d4', '#ec4899']
            }]
        }
    },

    // Learner Profile - Program Data
    program: {
        track: {
            labels: ['Foundations', 'Intermediate', 'Advanced', 'Specialization'],
            datasets: [{
                data: [5200, 3800, 2400, 1600],
                backgroundColor: ['#06b6d4', '#3b82f6', '#a855f7', '#ef4444']
            }]
        },
        channel: {
            labels: ['Online', 'In-Person', 'Hybrid', 'Self-Paced'],
            datasets: [{
                data: [7100, 2300, 2100, 1500],
                backgroundColor: ['#10b981', '#3b82f6', '#f59e0b', '#a855f7']
            }]
        },
        skill: {
            labels: ['Beginner', 'Intermediate', 'Advanced', 'Expert'],
            datasets: [{
                data: [4200, 5100, 2600, 1100],
                backgroundColor: ['#06b6d4', '#3b82f6', '#a855f7', '#ef4444']
            }]
        }
    },

    // Learner Profile - Provider Status
    provider: {
        labels: ['Microsoft', 'Oracle'],
        datasets: [{
            data: [58, 42],
            backgroundColor: ['#0078d4', '#f80000']
        }]
    },

    // Learner Profiles (Mock Data)
    learners: [
        { id: 1, name: 'Ahmed Hassan', email: 'ahmed.hassan@example.com', channel: 'NGO', status: 'Active', joinDate: '2024-01-15' },
        { id: 2, name: 'Fatima Ali', email: 'fatima.ali@example.com', channel: 'University', status: 'Active', joinDate: '2024-02-20' },
        { id: 3, name: 'Mohammed Ibrahim', email: 'm.ibrahim@example.com', channel: 'Employer', status: 'Completed', joinDate: '2023-12-10' },
        { id: 4, name: 'Layla Mohamed', email: 'layla.m@example.com', channel: 'Public Sector', status: 'Active', joinDate: '2024-01-05' },
        { id: 5, name: 'Karim Nassar', email: 'karim.nassar@example.com', channel: 'Syndicate', status: 'Active', joinDate: '2024-03-01' },
        { id: 6, name: 'Amira Khalil', email: 'amira.k@example.com', channel: 'University', status: 'Active', joinDate: '2024-02-15' },
        { id: 7, name: 'Hana Samir', email: 'hana.samir@example.com', channel: 'NGO', status: 'Inactive', joinDate: '2023-11-20' },
        { id: 8, name: 'Omar Saleh', email: 'omar.s@example.com', channel: 'Employer', status: 'Active', joinDate: '2024-01-25' },
        { id: 9, name: 'Noor Basal', email: 'noor.basal@example.com', channel: 'NGO', status: 'Active', joinDate: '2024-03-10' },
        { id: 10, name: 'Rami Khalil', email: 'rami.k@example.com', channel: 'Public Sector', status: 'Active', joinDate: '2024-02-05' },
    ]
};

// ========================================
// STATE MANAGEMENT
// ========================================

let currentView = 'dashboard';
let charts = {}; // Store chart instances for updates

// ========================================
// PAGE SWITCHING FUNCTION
// ========================================

function switchPage(pageName) {
    // Hide all pages
    document.querySelectorAll('.page-content').forEach(page => {
        page.classList.add('hidden');
    });

    // Show selected page
    const targetPage = document.getElementById(`page-${pageName}`);
    if (targetPage) {
        targetPage.classList.remove('hidden');
    }

    // Update current view
    currentView = pageName;

    // Update sidebar active state (only for main categories)
    const mainCategories = ['dashboard', 'national-coverage', 'insights', 'geographic', 'learner-profile'];
    document.querySelectorAll('.nav-link').forEach(link => {
        link.classList.remove('active');
    });
    
    let activeCategory = pageName;
    if (!mainCategories.includes(pageName)) {
        // Extract category from subcategory page name (e.g., 'nc-registration' -> 'national-coverage')
        const categoryMap = {
            'nc-registration': 'national-coverage',
            'nc-capacities': 'national-coverage',
            'nc-growth': 'national-coverage',
            'in-interest': 'insights',
            'in-motivations': 'insights',
            'in-challenges': 'insights',
            'gc-regional': 'geographic',
            'gc-gap': 'geographic',
            'gc-effectiveness': 'geographic',
            'lp-search': 'learner-profile',
            'lp-demographics': 'learner-profile',
            'lp-program': 'learner-profile',
            'lp-provider': 'learner-profile'
        };
        activeCategory = categoryMap[pageName] || 'dashboard';
    }
    
    document.querySelector(`[data-page="${activeCategory}"]`).classList.add('active');

    // Update page title
    const titles = {
        'dashboard': 'Dashboard',
        'national-coverage': 'National Coverage',
        'nc-registration': 'Registration by Channels',
        'nc-capacities': 'Deep-Dive Capacities',
        'nc-growth': 'Growth Metrics',
        'insights': 'Insights',
        'in-interest': 'Areas of Interest',
        'in-motivations': 'Motivations',
        'in-challenges': 'Challenges',
        'geographic': 'Geographic Coverage',
        'gc-regional': 'Regional Distribution',
        'gc-gap': 'Gap Analysis',
        'gc-effectiveness': 'Channel Effectiveness',
        'learner-profile': 'Learner Profile',
        'lp-search': 'Learner Search',
        'lp-demographics': 'Demographics',
        'lp-program': 'Program Data',
        'lp-provider': 'Provider Status'
    };
    document.getElementById('page-title').textContent = titles[pageName] || 'Dashboard';

    // Initialize or update charts based on page
    if (pageName === 'dashboard') {
        initServiceRequestsChart();
    } else if (pageName === 'national-coverage') {
        // Category page, no charts
    } else if (pageName === 'nc-registration') {
        initRegistrationChart();
    } else if (pageName === 'nc-capacities') {
        // No charts needed
    } else if (pageName === 'nc-growth') {
        initGrowthMetricsChart();
    } else if (pageName === 'insights') {
        // Category page, no charts
    } else if (pageName === 'in-interest') {
        initInterestChart();
    } else if (pageName === 'in-motivations') {
        initMotivationsChart();
    } else if (pageName === 'in-challenges') {
        initChallengesChart();
    } else if (pageName === 'geographic') {
        // Category page, no charts
    } else if (pageName === 'gc-regional') {
        initRegionalChart();
    } else if (pageName === 'gc-gap') {
        // No charts needed, only progress bars
    } else if (pageName === 'gc-effectiveness') {
        initChannelEffectivenessChart();
    } else if (pageName === 'learner-profile') {
        // Category page, no charts
    } else if (pageName === 'lp-search') {
        populateLearnerTable();
    } else if (pageName === 'lp-demographics') {
        updateDemographicsChart();
    } else if (pageName === 'lp-program') {
        updateProgramChart();
    } else if (pageName === 'lp-provider') {
        initProviderChart();
    }
}

// ========================================
// CHART INITIALIZATION FUNCTIONS
// ========================================

function initServiceRequestsChart() {
    const ctx = document.getElementById('serviceRequestsChart');
    if (!ctx) return;

    if (charts.serviceRequests) {
        charts.serviceRequests.destroy();
    }

    charts.serviceRequests = new Chart(ctx, {
        type: 'line',
        data: mockData.serviceRequests,
        options: {
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    display: true,
                    position: 'top'
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function(value) {
                            return value.toLocaleString();
                        }
                    }
                }
            }
        }
    });
}

function initRegistrationChart() {
    const ctx = document.getElementById('registrationChart');
    if (!ctx) return;

    if (charts.registration) {
        charts.registration.destroy();
    }

    charts.registration = new Chart(ctx, {
        type: 'doughnut',
        data: mockData.registration,
        options: {
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    position: 'bottom'
                }
            }
        }
    });
}

function initGrowthMetricsChart() {
    const ctx = document.getElementById('growthMetricsChart');
    if (!ctx) return;

    if (charts.growthMetrics) {
        charts.growthMetrics.destroy();
    }

    const filterValue = document.getElementById('channelFilter').value || 'all';
    const data = mockData.growthMetrics[filterValue] || mockData.growthMetrics.all;

    charts.growthMetrics = new Chart(ctx, {
        type: 'line',
        data: data,
        options: {
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    display: true,
                    position: 'top'
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function(value) {
                            return value.toLocaleString();
                        }
                    }
                }
            }
        }
    });
}

function initInsightsCharts() {
    // Areas of Interest
    const interestCtx = document.getElementById('interestChart');
    if (interestCtx) {
        if (charts.interest) {
            charts.interest.destroy();
        }
        charts.interest = new Chart(interestCtx, {
            type: 'pie',
            data: mockData.interests,
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        position: 'bottom'
                    }
                }
            }
        });
    }

    // Motivations
    const motivationsCtx = document.getElementById('motivationsChart');
    if (motivationsCtx) {
        if (charts.motivations) {
            charts.motivations.destroy();
        }
        charts.motivations = new Chart(motivationsCtx, {
            type: 'bar',
            data: mockData.motivations,
            options: {
                indexAxis: 'y',
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    x: {
                        beginAtZero: true
                    }
                }
            }
        });
    }

    // Challenges
    const challengesCtx = document.getElementById('challengesChart');
    if (challengesCtx) {
        if (charts.challenges) {
            charts.challenges.destroy();
        }
        charts.challenges = new Chart(challengesCtx, {
            type: 'bar',
            data: mockData.challenges,
            options: {
                indexAxis: 'y',
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    x: {
                        beginAtZero: true
                    }
                }
            }
        });
    }
}

function initInterestChart() {
    const ctx = document.getElementById('interestChart');
    if (!ctx) return;

    if (charts.interest) {
        charts.interest.destroy();
    }

    charts.interest = new Chart(ctx, {
        type: 'pie',
        data: mockData.interests,
        options: {
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    position: 'bottom'
                }
            }
        }
    });
}

function initMotivationsChart() {
    const ctx = document.getElementById('motivationsChart');
    if (!ctx) return;

    if (charts.motivations) {
        charts.motivations.destroy();
    }

    charts.motivations = new Chart(ctx, {
        type: 'bar',
        data: mockData.motivations,
        options: {
            indexAxis: 'y',
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                x: {
                    beginAtZero: true
                }
            }
        }
    });
}

function initChallengesChart() {
    const ctx = document.getElementById('challengesChart');
    if (!ctx) return;

    if (charts.challenges) {
        charts.challenges.destroy();
    }

    charts.challenges = new Chart(ctx, {
        type: 'bar',
        data: mockData.challenges,
        options: {
            indexAxis: 'y',
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                x: {
                    beginAtZero: true
                }
            }
        }
    });
}

function initGeographicCharts() {
    // Regional Distribution
    const regionalCtx = document.getElementById('regionalChart');
    if (regionalCtx) {
        if (charts.regional) {
            charts.regional.destroy();
        }
        charts.regional = new Chart(regionalCtx, {
            type: 'bar',
            data: mockData.regional,
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        display: false
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

    // Channel Effectiveness
    const effectivenessCtx = document.getElementById('channelEffectivenessChart');
    if (effectivenessCtx) {
        if (charts.channelEffectiveness) {
            charts.channelEffectiveness.destroy();
        }
        charts.channelEffectiveness = new Chart(effectivenessCtx, {
            type: 'bar',
            data: mockData.channelEffectiveness,
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        position: 'top'
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        stacked: false
                    },
                    x: {
                        stacked: false
                    }
                }
            }
        });
    }
}

function initRegionalChart() {
    const ctx = document.getElementById('regionalChart');
    if (!ctx) return;

    if (charts.regional) {
        charts.regional.destroy();
    }

    charts.regional = new Chart(ctx, {
        type: 'bar',
        data: mockData.regional,
        options: {
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    display: false
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

function initChannelEffectivenessChart() {
    const ctx = document.getElementById('channelEffectivenessChart');
    if (!ctx) return;

    if (charts.channelEffectiveness) {
        charts.channelEffectiveness.destroy();
    }

    charts.channelEffectiveness = new Chart(ctx, {
        type: 'bar',
        data: mockData.channelEffectiveness,
        options: {
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    position: 'top'
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    stacked: false
                },
                x: {
                    stacked: false
                }
            }
        }
    });
}

function initLearnerProfileCharts() {
    // Demographics
    updateDemographicsChart();

    // Program Data
    updateProgramChart();

    // Provider Status
    initProviderChart();
}

function initProviderChart() {
    const providerCtx = document.getElementById('providerChart');
    if (!providerCtx) return;

    if (charts.provider) {
        charts.provider.destroy();
    }
    charts.provider = new Chart(providerCtx, {
        type: 'doughnut',
        data: mockData.provider,
        options: {
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    position: 'bottom'
                }
            }
        }
    });
}

function updateDemographicsChart() {
    const filterValue = document.getElementById('demographicsFilter').value || 'age';
    const data = mockData.demographics[filterValue] || mockData.demographics.age;

    const ctx = document.getElementById('demographicsChart');
    if (!ctx) return;

    if (charts.demographics) {
        charts.demographics.destroy();
    }

    charts.demographics = new Chart(ctx, {
        type: 'doughnut',
        data: data,
        options: {
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    position: 'bottom'
                }
            }
        }
    });
}

function updateProgramChart() {
    const filterValue = document.getElementById('programFilter').value || 'track';
    const data = mockData.program[filterValue] || mockData.program.track;

    const ctx = document.getElementById('programChart');
    if (!ctx) return;

    if (charts.program) {
        charts.program.destroy();
    }

    charts.program = new Chart(ctx, {
        type: 'doughnut',
        data: data,
        options: {
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    position: 'bottom'
                }
            }
        }
    });
}

function updateGrowthChart() {
    if (currentView === 'nc-growth') {
        const ctx = document.getElementById('growthMetricsChart');
        if (!ctx) return;

        if (charts.growthMetrics) {
            charts.growthMetrics.destroy();
        }

        const filterValue = document.getElementById('channelFilter').value || 'all';
        const data = mockData.growthMetrics[filterValue] || mockData.growthMetrics.all;

        charts.growthMetrics = new Chart(ctx, {
            type: 'line',
            data: data,
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        display: true,
                        position: 'top'
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            callback: function(value) {
                                return value.toLocaleString();
                            }
                        }
                    }
                }
            }
        });
    }
}

// ========================================
// LEARNER TABLE FUNCTIONS
// ========================================

function populateLearnerTable() {
    const tbody = document.getElementById('learnerTableBody');
    if (!tbody) return;

    tbody.innerHTML = mockData.learners.map(learner => {
        const statusColor = learner.status === 'Active' ? 'badge-success' : learner.status === 'Completed' ? 'badge-success' : 'badge-warning';
        return `
            <tr>
                <td class="px-4 py-3 font-semibold">${learner.name}</td>
                <td class="px-4 py-3">${learner.email}</td>
                <td class="px-4 py-3">${learner.channel}</td>
                <td class="px-4 py-3"><span class="badge ${statusColor}">${learner.status}</span></td>
                <td class="px-4 py-3 text-gray-600">${learner.joinDate}</td>
            </tr>
        `;
    }).join('');

    // Add search functionality
    document.getElementById('learnerSearch').addEventListener('input', filterLearnerTable);
}

function filterLearnerTable() {
    const searchValue = document.getElementById('learnerSearch').value.toLowerCase();
    const tbody = document.getElementById('learnerTableBody');
    const rows = tbody.querySelectorAll('tr');

    rows.forEach(row => {
        const text = row.textContent.toLowerCase();
        row.style.display = text.includes(searchValue) ? '' : 'none';
    });
}

// ========================================
// INITIALIZATION
// ========================================

document.addEventListener('DOMContentLoaded', function() {
    // Initialize dashboard on load
    switchPage('dashboard');

    // Initialize charts for dashboard
    setTimeout(() => {
        initServiceRequestsChart();
    }, 100);
});

// ========================================
// RESPONSIVE UTILITIES
// ========================================

// Handle window resize for charts
window.addEventListener('resize', function() {
    Object.keys(charts).forEach(key => {
        if (charts[key] && charts[key].resize) {
            charts[key].resize();
        }
    });
});
