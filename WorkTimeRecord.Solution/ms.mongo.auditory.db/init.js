// Contexto Base de datos MongoDB para la aplicaci�n Auditory   
db = db.getSiblingDB("AuditoryDB");

// Crear Usuario y permisos
db.createUser({
    user: "admin",
    pwd: "admin",
    roles: [
        { role: "readWrite", db: "AuditoryDB" }, 
    ]
}); 

// Crear Colecciones
db.createCollection('UserRecord');