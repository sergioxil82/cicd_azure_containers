import { defineStore } from "pinia";
import { ref } from "vue";

export const useRecordsAuditStore = defineStore("recordsAudit", () => {
    const records = ref([]);
    const apiUrl = import.meta.env.VITE_API_WORK_TIME_URL // Accede a la URL de la API desde .env
    const userName  = import.meta.env.VITE_USER_NAME // Accede al nombre de usuario desde .env
   
    const mockedItems = [{ userName: "rserrano", firstName: "Ramón", lastName: "Serrano" , lastRecord: new Date("2023-10-01T12:00:00Z").toLocaleString(), mode: 'Entrada'},
        { userName: "jdoe", firstName: "John", lastName: "Doe" , lastRecord: new Date("2023-10-01T12:00:00Z").toLocaleString(), mode: 'Salida'},
        { userName: "asmith", firstName: "Alice", lastName: "Smith" , lastRecord: new Date("2023-10-01T12:00:00Z").toLocaleString(), mode: 'Entrada'},
        { userName: "bwhite", firstName: "Bob", lastName: "White" , lastRecord: new Date("2023-10-01T12:00:00Z").toLocaleString(), mode: 'Salida' }]
    
        /*
    const FakeAPI = {
        async fetch() {
            return new Promise((resolve) => {
                setTimeout(() => {
                    resolve({items: mockedItems});
                }, 1500);
            });
        }
    }
    async function getRecords() {
        const result = await FakeAPI.fetch();
        records.value = result.items
    }*/
   const API = {
        async fetch() {
            try {
                // GET Registry API
                console.log('GET Registry API apiUrl', apiUrl, 'userName', userName);
                const response = await fetch(`${apiUrl}/UserWorkTimeRecord/${userName}`);
                console.log('GET Registry API response', response);
                if (!response.ok) {
                    throw new Error(`Error: ${response.status}`);
                }
                const data = await response.json();
                console.log('GET Registry API data', data);
                return {items: data};
            } catch (error) {
                console.error("Error fetching records:", error);
                return {items: []}; // Retorna un array vacío en caso de error
            }
        }
    }

    async function getRecords() {
        const result = await API.fetch();
        // Asegura que siempre es un array
        const items = Array.isArray(result.items)
            ? result.items
            : result.items
                ? [result.items]
                : [];
        records.value = items.map((record) => ({
            ...record,
            lastRecord: record.lastRecord ? new Date(record.lastRecord).toLocaleString() : ''
        }));
    }

    return {
        records,
        getRecords
    }
});