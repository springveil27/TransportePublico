const API_BASE_URL = 'https://localhost:7119/api'; 

let currentRoutes = [];
let currentStops = [];
let currentSchedules = [];

const elements = {
    loading: document.getElementById('loading'),
    alert: document.getElementById('alert'),
    routesList: document.getElementById('routesList'),
    stopsList: document.getElementById('stopsList'),
    schedulesList: document.getElementById('schedulesList'),
    routeSearchInput: document.getElementById('routeSearchInput'),
    stopSearchInput: document.getElementById('stopSearchInput'),
    stopRouteFilter: document.getElementById('stopRouteFilter'),
    scheduleRouteFilter: document.getElementById('scheduleRouteFilter')
};

function showLoading() {
    if (elements.loading) elements.loading.style.display = 'flex';
}

function hideLoading() {
    if (elements.loading) elements.loading.style.display = 'none';
}

function showAlert(message, type = 'success') {
    if (!elements.alert) return;
    elements.alert.textContent = message;
    elements.alert.className = `alert alert-${type} show`;
    setTimeout(() => {
        elements.alert.classList.remove('show');
    }, 5000);
}

async function apiRequest(url, options = {}) {
    try {
        showLoading();
        console.log('Making request to:', url, 'with options:', options);
        const response = await fetch(url, {
            headers: {
                'Content-Type': 'application/json',
                ...options.headers
            },
            ...options
        });
        console.log('Response status:', response.status, 'for URL:', url);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        let data = null;
        const contentType = response.headers.get('content-type');
        if (contentType && contentType.includes('application/json')) {
            const text = await response.text();
            if (text) {
                data = JSON.parse(text);
            }
        }
        return data;
    } catch (error) {
        console.error('API Request Error:', error);
        showAlert(`Error: ${error.message}`, 'error');
        throw error;
    } finally {
        hideLoading();
    }
}

function showTab(tabName) {
    document.querySelectorAll('.tab-content').forEach(tab => {
        tab.classList.remove('active');
    });
    document.querySelectorAll('.nav-tab').forEach(btn => {
        btn.classList.remove('active');
    });
    const targetTab = document.getElementById(`${tabName}-tab`);
    if (targetTab) {
        targetTab.classList.add('active');
    }
    event.target.classList.add('active');
    switch(tabName) {
        case 'routes':
            loadRoutes();
            break;
        case 'stops':
            loadStops();
            break;
        case 'schedules':
            loadSchedules();
            break;
    }
}

async function loadRoutes() {
    try {
        const routes = await apiRequest(`${API_BASE_URL}/routes`);
        currentRoutes = routes || [];
        displayRoutes(currentRoutes);
        loadRoutesForSelect();
    } catch (error) {
        console.error('Error loading routes:', error);
        showAlert('Error al cargar las rutas', 'error');
    }
}

function displayRoutes(routes) {
    if (!elements.routesList) return;
    if (!routes || routes.length === 0) {
        elements.routesList.innerHTML = '<div class="no-data">No hay rutas disponibles</div>';
        return;
    }
    const routesHTML = routes.map(route => `
    <div class="data-card">
        <div class="card-header">
            <h3>${route.name}</h3>
            <div class="card-actions">
                <button class="btn btn-sm btn-secondary" onclick="editRoute(${route.id})">
                    <i class="fas fa-edit"></i> Editar
                </button>
                <button class="btn btn-sm btn-danger" onclick="deleteRoute(${route.id})">
                    <i class="fas fa-trash"></i> Eliminar
                </button>
            </div>
        </div>
        <div class="card-body">
            <p><strong>Código:</strong> ${route.code}</p>
            <p><strong>Origen:</strong> ${route.origin || 'N/A'}</p>
            <p><strong>Destino:</strong> ${route.destination || 'N/A'}</p>
            <p><strong>Costo:</strong> RD$ ${route.cost.toFixed(2)}</p>
            <p><strong>Estado:</strong> 
                <span class="status ${route.active ? 'active' : 'inactive'}">
                    ${route.active ? 'Activa' : 'Inactiva'}
                </span>
            </p>
        </div>
    </div>
`).join('');
    elements.routesList.innerHTML = routesHTML;
}

function showAddRouteModal() {
    const modal = document.getElementById('routeModal');
    const title = document.getElementById('routeModalTitle');
    const form = document.getElementById('routeForm');
    if (title) title.textContent = 'Agregar Nueva Ruta';
    if (form) form.reset();
    document.getElementById('routeId').value = '';
    if (modal) modal.style.display = 'flex';
}

async function editRoute(id) {
    try {
        const route = await apiRequest(`${API_BASE_URL}/routes/${id}`);
        const modal = document.getElementById('routeModal');
        const title = document.getElementById('routeModalTitle');
        if (title) title.textContent = 'Editar Ruta';
        document.getElementById('routeId').value = route.id;
        document.getElementById('routeName').value = route.name;
        document.getElementById('routeOrigin').value = route.origin;
        document.getElementById('routeDestination').value = route.destination;
        document.getElementById('routeCode').value = route.code;
        document.getElementById('routeCost').value = route.cost;
        document.getElementById('routeActive').value = route.active.toString();
        if (modal) modal.style.display = 'flex';
    } catch (error) {
        console.error('Error loading route:', error);
        showAlert('Error al cargar la ruta', 'error');
    }
}

async function deleteRoute(id) {
    const confirmed = await showConfirmDialog('¿Estás seguro de eliminar esta ruta?');
    if (!confirmed) return;
    try {
        await apiRequest(`${API_BASE_URL}/routes/${id}`, {
            method: 'DELETE'
        });
        showAlert('Ruta eliminada exitosamente');
        loadRoutes();
    } catch (error) {
        console.error('Error deleting route:', error);
        showAlert('Error al eliminar la ruta', 'error');
    }
}

async function loadStops() {
    try {
        let allStops = [];
        if (currentRoutes.length === 0) {
            await loadRoutes();
        }
        for (const route of currentRoutes) {
            try {
                const routeStops = await apiRequest(`${API_BASE_URL}/routes/${route.id}/stops`);
                if (routeStops && routeStops.length > 0) {
                    const stopsWithRoute = routeStops.map(stop => ({
                        ...stop,
                        publicRouteId: route.id,
                        publicRouteName: route.name
                    }));
                    allStops = allStops.concat(stopsWithRoute);
                }
            } catch (error) {
                console.warn(`Error loading stops for route ${route.id}:`, error);
            }
        }
        currentStops = allStops;
        displayStops(currentStops);
        await loadRoutesForSelect();
    } catch (error) {
        console.error('Error loading stops:', error);
        showAlert('Error al cargar las paradas', 'error');
    }
}

function displayStops(stops) {
    if (!elements.stopsList) return;
    if (!stops || stops.length === 0) {
        elements.stopsList.innerHTML = '<div class="no-data">No hay paradas disponibles</div>';
        return;
    }
    const stopsHTML = stops.map(stop => `
        <div class="data-card">
            <div class="card-header">
                <h3>${stop.name}</h3>
                <div class="card-actions">
                    <button class="btn btn-sm btn-secondary" onclick="editStop(${stop.publicRouteId}, ${stop.id})">
                        <i class="fas fa-edit"></i> Editar
                    </button>
                    <button class="btn btn-sm btn-danger" onclick="deleteStop(${stop.id})">
                        <i class="fas fa-trash"></i> Eliminar
                    </button>
                </div>
            </div>
            <div class="card-body">
                <p><strong>Ruta:</strong> ${stop.publicRouteName || 'N/A'}</p>
                <p><strong>Dirección:</strong> ${stop.address || 'N/A'}</p>
                <p><strong>Orden:</strong> ${stop.order}</p>
            </div>
        </div>
    `).join('');
    elements.stopsList.innerHTML = stopsHTML;
}

function showAddStopModal() {
    const modal = document.getElementById('stopModal');
    const title = document.getElementById('stopModalTitle');
    const form = document.getElementById('stopForm');
    if (title) title.textContent = 'Agregar Nueva Parada';
    if (form) form.reset();
    document.getElementById('stopId').value = '';
    if (modal) modal.style.display = 'flex';
}

async function editStop(routeId, stopId) {
    try {
        const stop = await apiRequest(`${API_BASE_URL}/stops/${stopId}`);
        const modal = document.getElementById('stopModal');
        const title = document.getElementById('stopModalTitle');
        if (title) title.textContent = 'Editar Parada';
        document.getElementById('stopId').value = stop.id;
        document.getElementById('stopRouteId').value = stop.publicRouteId;
        document.getElementById('stopName').value = stop.name;
        document.getElementById('stopOrder').value = stop.order;
        document.getElementById('stopAddress').value = stop.address || '';
        if (modal) modal.style.display = 'flex';
    } catch (error) {
        console.error('Error loading stop:', error);
        showAlert('Error al cargar la parada', 'error');
    }
}

async function deleteStop(id) {
    const confirmed = await showConfirmDialog('¿Estás seguro de eliminar esta parada?');
    if (!confirmed) return;
    try {
        await apiRequest(`${API_BASE_URL}/stops/${id}`, {
            method: 'DELETE'
        });
        showAlert('Parada eliminada exitosamente');
        loadStops();
    } catch (error) {
        console.error('Error deleting stop:', error);
        showAlert('Error al eliminar la parada', 'error');
    }
}

async function loadSchedules() {
    try {
        let allSchedules = [];
        if (currentRoutes.length === 0) {
            await loadRoutes();
        }
        for (const route of currentRoutes) {
            try {
                const routeSchedules = await apiRequest(`${API_BASE_URL}/routes/${route.id}/schedules`);
                if (routeSchedules && routeSchedules.length > 0) {
                    const schedulesWithRoute = routeSchedules.map(schedule => ({
                        ...schedule,
                        publicRouteId: route.id,
                        routeName: route.name
                    }));
                    allSchedules = allSchedules.concat(schedulesWithRoute);
                }
            } catch (error) {
                console.warn(`Error loading schedules for route ${route.id}:`, error);
            }
        }
        currentSchedules = allSchedules;
        displaySchedules(currentSchedules);
        await loadRoutesForSelect();
    } catch (error) {
        console.error('Error loading schedules:', error);
        showAlert('Error al cargar los horarios', 'error');
    }
}

function displaySchedules(schedules) {
    if (!elements.schedulesList) return;
    if (!schedules || schedules.length === 0) {
        elements.schedulesList.innerHTML = '<div class="no-data">No hay horarios disponibles</div>';
        return;
    }
    const schedulesHTML = schedules.map(schedule => `
        <div class="data-card">
            <div class="card-header">
                <h3>Horario - ${schedule.routeName || 'N/A'}</h3>
                <div class="card-actions">
                    <button class="btn btn-sm btn-secondary" onclick="editSchedule(${schedule.id})">
                        <i class="fas fa-edit"></i> Editar
                    </button>
                    <button class="btn btn-sm btn-danger" onclick="deleteSchedule(${schedule.publicRouteId}, ${schedule.id})">
                        <i class="fas fa-trash"></i> Eliminar
                    </button>
                </div>
            </div>
            <div class="card-body">
                <p><strong>Ruta:</strong> ${schedule.routeName || 'N/A'}</p>
                <p><strong>Hora de inicio:</strong> ${formatTime(schedule.startTime)}</p>
                <p><strong>Hora de fin:</strong> ${formatTime(schedule.endTime)}</p>
                <p><strong>Frecuencia:</strong> ${schedule.frequencyMinutes} minutos</p>
            </div>
        </div>
    `).join('');
    elements.schedulesList.innerHTML = schedulesHTML;
}

function showAddScheduleModal() {
    const modal = document.getElementById('scheduleModal');
    const title = document.getElementById('scheduleModalTitle');
    const form = document.getElementById('scheduleForm');
    if (title) title.textContent = 'Agregar Nuevo Horario';
    if (form) form.reset();
    document.getElementById('scheduleId').value = '';
    if (modal) modal.style.display = 'flex';
}

async function editSchedule(scheduleId) {
    try {
        const schedule = await apiRequest(`${API_BASE_URL}/schedules/${scheduleId}`);
        const modal = document.getElementById('scheduleModal');
        const title = document.getElementById('scheduleModalTitle');
        if (title) title.textContent = 'Editar Horario';
        document.getElementById('scheduleId').value = schedule.id;
        document.getElementById('scheduleRouteId').value = schedule.publicRouteId;
        document.getElementById('scheduleStartTime').value = formatTimeForInput(schedule.startTime);
        document.getElementById('scheduleEndTime').value = formatTimeForInput(schedule.endTime);
        document.getElementById('scheduleFrequency').value = schedule.frequencyMinutes;
        if (modal) modal.style.display = 'flex';
    } catch (error) {
        console.error('Error loading schedule:', error);
        showAlert('Error al cargar el horario', 'error');
    }
}

async function deleteSchedule(routeId, scheduleId) {
    const confirmed = await showConfirmDialog('¿Estás seguro de eliminar este horario?');
    if (!confirmed) return;
    try {
        await apiRequest(`${API_BASE_URL}/routes/${routeId}/schedules/${scheduleId}`, {
            method: 'DELETE'
        });
        showAlert('Horario eliminado exitosamente');
        loadSchedules();
    } catch (error) {
        console.error('Error deleting schedule:', error);
        showAlert('Error al eliminar el horario', 'error');
    }
}

document.addEventListener('DOMContentLoaded', function() {
    const routeForm = document.getElementById('routeForm');
    if (routeForm) {
        routeForm.addEventListener('submit', async function(e) {
            e.preventDefault();
            const submitBtn = document.getElementById('routeSubmitBtn');
            setButtonLoading(submitBtn, true);
            const formData = new FormData(routeForm);
            if (!formData.get('name') || !formData.get('code') || !formData.get('origin') || !formData.get('destination')) {
                showAlert('Todos los campos son obligatorios', 'error');
                return;
            }
            if (isNaN(parseFloat(formData.get('cost'))) || parseFloat(formData.get('cost')) <= 0) {
                showAlert('El costo debe ser mayor a 0', 'error');
                return;
            }
            const routeData = {
                name: formData.get('name'),
                code: formData.get('code'),
                origin: formData.get('origin'),         
                destination: formData.get('destination'), 
                cost: parseFloat(formData.get('cost')),
                active: formData.get('active') === 'true'
            };
            const routeId = document.getElementById('routeId').value;
            try {
                if (routeId) {
                    routeData.id = parseInt(routeId);
                    await apiRequest(`${API_BASE_URL}/routes/${routeId}`, {
                        method: 'PUT',
                        body: JSON.stringify(routeData)
                    });
                    showAlert('Ruta actualizada exitosamente');
                } else {
                    await apiRequest(`${API_BASE_URL}/routes`, {
                        method: 'POST',
                        body: JSON.stringify(routeData)
                    });
                    showAlert('Ruta creada exitosamente');
                }
                closeModal('routeModal');
                loadRoutes();
            } catch (error) {
                console.error('Error saving route:', error);
                showAlert('Error al guardar la ruta', 'error');
            } finally {
                setButtonLoading(submitBtn, false);
            }
        });
    }
    const stopForm = document.getElementById('stopForm');
    if (stopForm) {
        stopForm.addEventListener('submit', async function(e) {
            e.preventDefault();
            const formData = new FormData(stopForm);
            if (!formData.get('name') || !formData.get('address') || !formData.get('order')) {
                showAlert('Todos los campos son obligatorios', 'error');
                return;
            }
            if (isNaN(parseInt(formData.get('order'))) || parseInt(formData.get('order')) < 1) {
                showAlert('El orden debe ser mayor o igual a 1', 'error');
                return;
            }
            const stopData = {
                name: formData.get('name'),
                address: formData.get('address'),
                order: parseInt(formData.get('order'))
            };
            const stopId = document.getElementById('stopId').value;
            const routeId = formData.get('routeId');
            if (!routeId) {
                showAlert('Por favor selecciona una ruta', 'error');
                return;
            }
            try {
                if (stopId) {
                    await apiRequest(`${API_BASE_URL}/routes/${routeId}/stops/${stopId}`, {
                        method: 'PUT',
                        body: JSON.stringify(stopData)
                    });
                    showAlert('Parada actualizada exitosamente');
                } else {
                    await apiRequest(`${API_BASE_URL}/routes/${routeId}/stops`, {
                        method: 'POST',
                        body: JSON.stringify(stopData)
                    });
                    showAlert('Parada creada exitosamente');
                }
                closeModal('stopModal');
                loadStops();
            } catch (error) {
                console.error('Error saving stop:', error);
                showAlert('Error al guardar la parada', 'error');
            }
        });
    }
    const scheduleForm = document.getElementById('scheduleForm');
    if (scheduleForm) {
        scheduleForm.addEventListener('submit', async function(e) {
            e.preventDefault();
            const formData = new FormData(scheduleForm);
            if (!formData.get('startTime') || !formData.get('endTime') || !formData.get('frequencyMinutes')) {
                showAlert('Todos los campos son obligatorios', 'error');
                return;
            }
            if (isNaN(parseInt(formData.get('frequencyMinutes'))) || parseInt(formData.get('frequencyMinutes')) <= 0) {
                showAlert('La frecuencia debe ser mayor a 0', 'error');
                return;
            }
            const scheduleData = {
                startTime: formData.get('startTime'),
                endTime: formData.get('endTime'),
                frequencyMinutes: parseInt(formData.get('frequencyMinutes'))
            };
            const scheduleId = document.getElementById('scheduleId').value;
            const routeId = formData.get('routeId');
            if (!routeId) {
                showAlert('Por favor selecciona una ruta', 'error');
                return;
            }
            try {
                if (scheduleId) {
                    await apiRequest(`${API_BASE_URL}/routes/${routeId}/schedules/${scheduleId}`, {
                        method: 'PUT',
                        body: JSON.stringify(scheduleData)
                    });
                    showAlert('Horario actualizado exitosamente');
                } else {
                    await apiRequest(`${API_BASE_URL}/routes/${routeId}/schedules`, {
                        method: 'POST',
                        body: JSON.stringify(scheduleData)
                    });
                    showAlert('Horario creado exitosamente');
                }
                closeModal('scheduleModal');
                loadSchedules();
            } catch (error) {
                console.error('Error saving schedule:', error);
                showAlert('Error al guardar el horario', 'error');
            }
        });
    }
});

function setButtonLoading(btn, loading) {
    if (!btn) return;
    if (loading) {
        btn.disabled = true;
        btn.innerHTML = '<span class="spinner-btn"></span> Guardando...';
    } else {
        btn.disabled = false;
        btn.innerHTML = '<i class="fas fa-save"></i> Guardar Ruta';
    }
}

function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) modal.style.display = 'none';
}

function formatTime(timeString) {
    if (!timeString) return 'N/A';
    if (typeof timeString === 'string' && timeString.includes(':')) {
        const parts = timeString.split(':');
        return `${parts[0]}:${parts[1]}`;
    }
    return timeString;
}

function formatTimeForInput(timeString) {
    if (!timeString) return '';
    if (typeof timeString === 'string' && timeString.includes(':')) {
        const parts = timeString.split(':');
        return `${parts[0].padStart(2, '0')}:${parts[1].padStart(2, '0')}`;
    }
    return timeString;
}

async function loadRoutesForSelect() {
    try {
        const routes = currentRoutes.length > 0 ? currentRoutes : await apiRequest(`${API_BASE_URL}/routes`);
        const stopRouteSelect = document.getElementById('stopRouteId');
        const stopRouteFilter = document.getElementById('stopRouteFilter');
        const scheduleRouteSelect = document.getElementById('scheduleRouteId');
        const scheduleRouteFilter = document.getElementById('scheduleRouteFilter');
        const routeOptions = routes.map(route => 
            `<option value="${route.id}">${route.name} (${route.code})</option>`
        ).join('');
        if (stopRouteSelect) {
            stopRouteSelect.innerHTML = '<option value="">Seleccione una ruta</option>' + routeOptions;
        }
        if (stopRouteFilter) {
            stopRouteFilter.innerHTML = '<option value="">Todas las rutas</option>' + routeOptions;
        }
        if (scheduleRouteSelect) {
            scheduleRouteSelect.innerHTML = '<option value="">Seleccione una ruta</option>' + routeOptions;
        }
        if (scheduleRouteFilter) {
            scheduleRouteFilter.innerHTML = '<option value="">Todas las rutas</option>' + routeOptions;
        }
    } catch (error) {
        console.error('Error loading routes for select:', error);
    }
}

function showConfirmDialog(message) {
    return new Promise((resolve) => {
        const modal = document.getElementById('confirmModal');
        const messageElement = document.getElementById('confirmMessage');
        const confirmBtn = document.getElementById('confirmBtn');
        if (messageElement) messageElement.textContent = message;
        if (modal) modal.style.display = 'flex';
        const handleConfirm = () => {
            closeModal('confirmModal');
            confirmBtn.removeEventListener('click', handleConfirm);
            resolve(true);
        };
        const handleCancel = () => {
            closeModal('confirmModal');
            resolve(false);
        };
        if (confirmBtn) {
            confirmBtn.addEventListener('click', handleConfirm);
        }
        modal.addEventListener('click', (e) => {
            if (e.target === modal) {
                handleCancel();
            }
        });
    });
}

if (elements.routeSearchInput) {
    elements.routeSearchInput.addEventListener('input', function() {
        const searchTerm = this.value.toLowerCase();
        const filteredRoutes = currentRoutes.filter(route => 
            route.name.toLowerCase().includes(searchTerm) ||
            route.code.toLowerCase().includes(searchTerm)
        );
        displayRoutes(filteredRoutes);
    });
}

if (elements.stopSearchInput) {
    elements.stopSearchInput.addEventListener('input', function() {
        const searchTerm = this.value.toLowerCase();
        const filteredStops = currentStops.filter(stop => 
            stop.name.toLowerCase().includes(searchTerm) ||
            (stop.address && stop.address.toLowerCase().includes(searchTerm))
        );
        displayStops(filteredStops);
    });
}

if (elements.stopRouteFilter) {
    elements.stopRouteFilter.addEventListener('change', function() {
        const routeId = this.value;
        if (routeId) {
            const filteredStops = currentStops.filter(stop => 
                stop.publicRouteId == routeId
            );
            displayStops(filteredStops);
        } else {
            displayStops(currentStops);
        }
    });
}

if (elements.scheduleRouteFilter) {
    elements.scheduleRouteFilter.addEventListener('change', function() {
        const routeId = this.value;
        if (routeId) {
            const filteredSchedules = currentSchedules.filter(schedule => 
                schedule.publicRouteId == routeId
            );
            displaySchedules(filteredSchedules);
        } else {
            displaySchedules(currentSchedules);
        }
    });
}

document.addEventListener('DOMContentLoaded', function() {
    loadRoutes();
    document.addEventListener('click', function(e) {
        if (e.target.classList.contains('modal')) {
            e.target.style.display = 'none';
        }
    });
    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape') {
            document.querySelectorAll('.modal').forEach(modal => {
                modal.style.display = 'none';
            });
        }
    });
});