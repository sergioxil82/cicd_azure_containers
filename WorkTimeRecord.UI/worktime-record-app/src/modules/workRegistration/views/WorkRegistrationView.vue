<template>
    <div class="work-registration">
        <div class="date-time">
            <p> <label class="date">{{ currentDate }}</label></p>
            <p> <label class="time">{{ currentTime }}</label></p>
        </div>
        <div class="buttons">
            <button @click="registerEntryClick" :disabled="lastMode == 'Entrada'" class="regiter-button">Registrar Entrada</button>
            <button @click="registerExitClick" :disabled="lastMode == 'Salida'" class="close-button">Registrar Salida</button>
        </div>
        <p> Último Regitro: {{ lastMode }} - {{ lastRecordDate?.time }} - {{ lastRecordDate?.date }}</p>
    </div>
</template>
<script setup>
// PINIA - Gestor de Estados
import { storeToRefs } from 'pinia';
import { useActivityStore } from '../stores/activityStore';
import { onMounted } from 'vue';

const activityStore = useActivityStore();
const { lastRecordDate, lastMode } = storeToRefs(activityStore);
const { getCurrentUserActivity, registerEntry, registerExit } = activityStore;

onMounted(() => {
    getCurrentUserActivity(); // Iniciar la llamada del store
});

const getCurrentDateTime = () => { // Función que exporta la fecha y hora actual
    const date = new Date();
    const dateOptions = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
    const timeOptions = { hour: '2-digit', minute: '2-digit' };
    const currentDate = date.toLocaleDateString('es-ES', dateOptions);
    const currentTime = date.toLocaleTimeString('es-ES', timeOptions);
    return { currentDate, currentTime };
    }

// Declaración de propiedades reactivas para fecha y hora actual
const { currentDate, currentTime } = getCurrentDateTime(); // Desestructuración de la función
// regintryEntry: Función para registrar la entrada del empleado
const registerEntryClick = () => { registerEntry(); }
// registerExit: Función para registrar la salida del empleado
const registerExitClick = () => { registerExit(); }
</script>

<style scoped>
.date-time {
    margin-top: 2rem;
    text-align: center;
    box-shadow: 0px 1px 1px #888;
    display: inline-block;
    border-radius: 15px;
    padding: 2rem;    
}
.work-registration { text-align: center; margin: 50px auto; }
.date {font-size: 20px;}
.time {font-size: 40px; font-weight: bold;}
.buttons { margin-top: 20px;}

button {
    padding: 10px 20px;
    font-size: 16px;
    border-radius: 5px;
    cursor: pointer;
    margin: 0 10px;
    border: none;
    transition: background-color 0.3s ease;
}
.register-button { background-color:  green; color: white;}
.close-button { background-color: red; color: white;}
button:hover { filter: brightness(1.2);}
button:disabled {
    background-color: gray;
    cursor: not-allowed;
    opacity: 0.5;
}
</style>