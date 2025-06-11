document.getElementById("formUsuario").addEventListener("submit", async function (e) {
  e.preventDefault();

  const usuario = {
    nombre: document.getElementById("nombre").value,
    email: document.getElementById("email").value,
    passwordHash: document.getElementById("password").value,
    saldoARS: parseFloat(document.getElementById("saldo").value)
  };

  const respuestaDiv = document.getElementById("respuesta");

  try {
    const res = await fetch("https://localhost:7244/api/usuario", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(usuario)
    });

    const data = await res.json();

    if (res.ok) {
      respuestaDiv.style.color = "green";
      respuestaDiv.textContent = `✅ Usuario creado: ${data.nombre} (ID: ${data.id})`;
    } else {
      respuestaDiv.style.color = "red";
      respuestaDiv.textContent = `❌ Error: ${data}`;
    }

  } catch (err) {
    respuestaDiv.style.color = "red";
    respuestaDiv.textContent = `❌ Error de red: ${err.message}`;
  }
});
