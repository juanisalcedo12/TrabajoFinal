async function cargarTransacciones() {
  const tabla = document.getElementById("tablaTransacciones");
  const cuerpo = document.getElementById("transaccionesBody");
  const mensaje = document.getElementById("mensaje");

  cuerpo.innerHTML = "";
  tabla.style.display = "none";
  mensaje.textContent = "Cargando transacciones...";

  try {
    const res = await fetch("https://localhost:7244/api/transaccion");

    if (!res.ok) {
      mensaje.textContent = "❌ No se pudieron obtener las transacciones.";
      return;
    }

    const transacciones = await res.json();

    if (transacciones.length === 0) {
      mensaje.textContent = "No hay transacciones registradas.";
      return;
    }

    transacciones.forEach(t => {
      const fila = document.createElement("tr");

      fila.innerHTML = `
        <td>${t.usuario}</td>
        <td>${t.cryptoCode}</td>
        <td>${t.exchange}</td>
        <td>${t.action}</td>
        <td>${t.cryptoAmount}</td>
        <td>$${t.montoARS.toFixed(2)}</td>
        <td>${new Date(t.fechaHora).toLocaleString()}</td>
      `;

      cuerpo.appendChild(fila);
    });

    mensaje.textContent = "";
    tabla.style.display = "table";
  } catch (err) {
    mensaje.textContent = `❌ Error: ${err.message}`;
  }
}

// Cargar al iniciar
document.addEventListener("DOMContentLoaded", cargarTransacciones);
