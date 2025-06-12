document.addEventListener("DOMContentLoaded", () => {
  cargarExchanges();
  cargarUsuarios();
});

document.getElementById("formTransaccion").addEventListener("submit", async function (e) {
  e.preventDefault();

  const transaccion = {
    usuarioId: parseInt(document.getElementById("usuarioId").value),
    cryptoCode: document.getElementById("cryptoCode").value.trim(),
    exchangeId: parseInt(document.getElementById("exchangeId").value),
    action: document.getElementById("action").value,
    cryptoAmount: parseFloat(document.getElementById("cryptoAmount").value),
    fechaHora: document.getElementById("fechaHora").value
  };

  const respuestaDiv = document.getElementById("respuesta");

  try {
    const res = await fetch("https://localhost:7244/api/transaccion", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(transaccion)
    });

    let data;
    try {
      data = await res.json();
    } catch {
      data = await res.text();
    }

    if (res.ok) {
      respuestaDiv.style.color = "green";
      respuestaDiv.textContent = `✅ Transacción registrada. ID: ${data.id}, Monto ARS: $${data.montoARS}`;
    } else {
      respuestaDiv.style.color = "red";
      respuestaDiv.textContent = `❌ Error: ${data.error || data}`;
    }

  } catch (err) {
    respuestaDiv.style.color = "red";
    respuestaDiv.textContent = `❌ Error de red: ${err.message}`;
  }
});

async function cargarExchanges() {
  const select = document.getElementById("exchangeId");

  try {
    const res = await fetch("https://localhost:7244/api/exchange");
    const exchanges = await res.json();

    exchanges.forEach(e => {
      const option = document.createElement("option");
      option.value = e.id;
      option.textContent = e.nombre;
      select.appendChild(option);
    });
  } catch (err) {
    console.error("❌ Error al cargar exchanges:", err.message);
  }
}

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
