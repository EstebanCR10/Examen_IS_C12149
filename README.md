# Examen_IS_C12149
Repo para el examen final Ingeniería de software de Esteban Chaves Obando C12149

# 🥤 Máquina Expendedora de frescos

Este proyecto es un sistema web de una máquina expendedora de refrescos. El sistema permite a los usuarios seleccionar bebidas, ingresar dinero en monedas o billetes, y recibir su vuelto calculado automáticamente. Toda la información se maneja **en memoria**, sin base de datos.

---

## 🛠️ Ejecución del Proyecto

### 1. Backend (.NET API)

```bash
cd backend
dotnet restore
dotnet build
dotnet run
```

Esto iniciará el backend en `http://localhost:5110`.

El Swagger UI estará disponible en:
```
http://localhost:5110/swagger
```

---

### 2. Frontend (Vue.js)

```bash
cd frontend
npm install
npm run dev
```

Esto abrirá la aplicación web en `http://localhost:5173`.

---

## 🧪 Funcionalidades

- Ver bebidas disponibles y sus cantidades
- Agregar bebidas al carrito
- Ingresar dinero con monedas y billetes (₡1000, ₡500, ₡100, ₡50, ₡25)
- Validación de monto ingresado
- Cálculo automático del vuelto
- popup con el desglose del cambio
- Botón para volver a comprar

