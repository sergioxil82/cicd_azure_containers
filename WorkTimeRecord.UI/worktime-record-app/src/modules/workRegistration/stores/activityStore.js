import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useActivityStore = defineStore('activity', () => {
    const lastRecordDate = ref()
    const lastMode = ref()

    // Añadir variables de entorno
    const apiUrl = import.meta.env.VITE_API_WORK_TIME_URL // Accede a la URL de la API desde .env
    const userName  = import.meta.env.VITE_USER_NAME // Accede al nombre de usuario desde .env
    const firstName = import.meta.env.VITE_FIRST_NAME // Accede al nombre desde .env
    const lastName  = import.meta.env.VITE_LAST_NAME // Accede al apellido desde .env

    const formatDateTime = (dateTime) => {
         if (!dateTime) return { date: '', time: '' }; // Maneja null/undefined
        const dateOptions = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
        const timeOptions = { hour: '2-digit', minute: '2-digit'}
        const localeDate = dateTime.toLocaleDateString('es-ES', dateOptions);
        const localeTime = dateTime.toLocaleTimeString('es-ES', timeOptions);
        return {date: localeDate, time: localeTime};
    }
    
    // Comentar las llamadas Mockeadas
    /*
    const FakeAPI = {
        async fetchGetLastActivity() {
            return new Promise((resolve) => { setTimeout(() => { resolve({recorDate: new Date(), lastMode: 'Entrada'})},500)})}, 
        async fetchRegisterEntry() {
            return new Promise((resolve) => { setTimeout(() => { resolve({recorDate: new Date(), lastMode: 'Entrada'})},500)})},
        async fetchRegisterExit() {
            return new Promise((resolve) => { setTimeout(() => { resolve({recorDate: new Date(), lastMode: 'Salida'})},500)})}
    }

    function getCurrentUserActivity() {
        FakeAPI.fetchGetLastActivity().then((result) => {
            lastRecordDate.value = formatDateTime(result.recorDate);
            lastMode.value = result.lastMode;
        })
    }
    function registerEntry() {
        FakeAPI.fetchRegisterEntry().then((result) => {
            lastRecordDate.value = formatDateTime(result.recorDate);
            lastMode.value = result.lastMode;
        })
    }
    function registerExit() {
        FakeAPI.fetchRegisterExit().then((result) => {
            lastRecordDate.value = formatDateTime(result.recorDate);
            lastMode.value = result.lastMode;
        })
    }
    */
   
    const API = {
        async fetchGetLastActivity() {
            try {
                // GET Registry API
                console.log('GET Registry API apiUrl', apiUrl, 'userName', userName);
                const response = await fetch(`${apiUrl}/UserWorkTimeRecord/${userName}`);
                 console.log('GET Registry API response', response);
                if (!response.ok) throw new Error('Error al obtener el último registro');
                // Obtener respuesta
                const result = await response.json();
                return {recorDate: new Date(result.lastRecord), lastMode: result.mode}; 
            } catch (error) {
                console.error('Error al obtener el último registro:', error);
                return {recorDate: null, lastMode: null};
            }
            
        },
        async fetchRegisterLastActivity(payload) {
            // POST Registry API
            const response = await fetch(`${apiUrl}/UserWorkTimeRecord`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(payload)
            });

            if (!response.ok) throw new Error('POST Error: ${response.status} ${response.statusText}');

            // Representar registro
            return {recorDate: new Date(payload.lastRecord), lastMode: payload.mode};
        }        
    }

    function getCurrentUserActivity() {
        API.fetchGetLastActivity().then((result) => {
            console.log(result);
            lastRecordDate.value = formatDateTime(result.recorDate);
            lastMode.value = result.lastMode;
        })
    }

    function register(mode) {
        const payload = {
            userName: userName,
            firstName: firstName,
            lastName: lastName,
            lastRecord: new Date(),
            mode: mode
        }
        API.fetchRegisterLastActivity(payload).then(() => {
            lastRecordDate.value = formatDateTime(payload.recorDate);
            lastMode.value = payload.lastMode;
        })
    }

    function registerEntry() {
        register('Entrada');
    }

    function registerExit() {
        register('Salida');
    }

    return {
        lastRecordDate,
        lastMode,
        getCurrentUserActivity,
        registerEntry,
        registerExit
    }
})