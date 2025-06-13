document.addEventListener("DOMContentLoaded", () => {
  // Asegura que todo el DOM está cargado
  cargarCriptomonedas();
  cargarExchanges();
  cargarUsuarios();

  const form = document.getElementById("formTransaccion");
  const respuestaDiv = document.getElementById("respuesta");

  if (!form) {
    console.error("⚠️ No se encontró el formulario con id='formTransaccion'");
    return;
  }

  form.addEventListener("submit", async function (e) {
    e.preventDefault();

    const transaccion = {
      usuarioId: parseInt(document.getElementById("usuarioId").value),
      cryptoCode: document.getElementById("cripto").value.trim(),
      exchangeId: parseInt(document.getElementById("exchangeId").value),
      action: document.getElementById("action").value,
      cryptoAmount: parseFloat(document.getElementById("cryptoAmount").value),
      fechaHora: document.getElementById("fechaHora").value
    };

    try {
      const res = await fetch("https://localhost:7244/api/transaccion", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(transaccion)
      });

      const raw = await res.text();
      let data;

      try {
        data = JSON.parse(raw);
      } catch {
        data = raw;
      }

      if (res.ok) {
        respuestaDiv.textContent = `✅ Transacción realizada. ID: ${data.id}, Monto ARS: $${data.montoARS}`;
      } else {
        respuestaDiv.textContent = `❌ Error: ${data}`;
      }

    } catch (err) {
      respuestaDiv.textContent = `❌ Error de red: ${err.message}`;
    }
  });
});

// Funciones auxiliares
async function cargarCriptomonedas() {
  try {
    const res = await fetch("https://localhost:7244/api/criptomoneda");
    const criptos = await res.json();

    const select = document.getElementById("cripto");
    criptos.forEach(c => {
      const option = document.createElement("option");
      option.value = c.codigo;
      option.textContent = ` ${c.codigo.toUpperCase()}`;
      select.appendChild(option);
    });
  } catch (err) {
    console.error("❌ Error al cargar criptomonedas:", err.message);
  }
}

async function cargarExchanges() {
  try {
    const res = await fetch("https://localhost:7244/api/exchange");
    const exchanges = await res.json();

    const select = document.getElementById("exchangeId");
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
  try {
    const res = await fetch("https://localhost:7244/api/usuario");
    const usuarios = await res.json();

    const select = document.getElementById("usuarioId");
    usuarios.forEach(u => {
      const option = document.createElement("option");
      option.value = u.id;
      option.textContent = u.nombre;
      select.appendChild(option);
    });
  } catch (err) {
    console.error("❌ Error al cargar usuarios:", err.message);
  }
}
