document.addEventListener("DOMContentLoaded", cargarUsuarios);

async function cargarUsuarios() {
  const select = document.getElementById("usuarioId");

  try {
    const res = await fetch("https://localhost:7244/api/usuario");
    const usuarios = await res.json();

    usuarios.forEach(u => {
      const option = document.createElement("option");
      option.value = u.id;
      option.textContent = `${u.nombre}`;
      select.appendChild(option);
    });
  } catch (err) {
    console.error("❌ Error al cargar usuarios:", err.message);
  }
}

async function consultarWallet() {
  const usuarioId = document.getElementById("usuarioId").value;
  const mensaje = document.getElementById("mensaje");
  const resultadoDiv = document.getElementById("resultado");
  const saldoArs = document.getElementById("saldoArs");
  const totalArs = document.getElementById("totalArs");
  const monedasBody = document.getElementById("monedasBody");

  mensaje.textContent = "";
  resultadoDiv.style.display = "none";
  monedasBody.innerHTML = "";

  if (!usuarioId) {
    mensaje.textContent = "❌ Seleccioná un usuario.";
    return;
  }

  try {
    const res = await fetch(`https://localhost:7244/api/usuario/wallet-status/${usuarioId}`);

    if (!res.ok) {
      const errText = await res.text();
      mensaje.textContent = `❌ Error: ${errText}`;
      return;
    }

    const data = await res.json();

    saldoArs.textContent = data.saldoARS.toFixed(2);
    totalArs.textContent = data.totalARS.toFixed(2);

    data.monedas.forEach(m => {
      const fila = document.createElement("tr");
      fila.innerHTML = `
        <td>${m.cryptoCode}</td>
        <td>${m.cantidad}</td>
        <td>$${m.valorEnPesos.toFixed(2)}</td>
      `;
      monedasBody.appendChild(fila);
    });

    resultadoDiv.style.display = "block";
  } catch (err) {
    mensaje.textContent = `❌ Error de red: ${err.message}`;
  }
}
