import React, { useState } from 'react';

const API_BASE_URL = "http://localhost:5069";

function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);
  // Estado para forçar a re-renderização após o login
  const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem('token'));

  const handleLogin = async (e) => {
    e.preventDefault();
    setError(null);

    try {
      const response = await fetch(`${API_BASE_URL}/api/User/login`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        // Se estiver usando Bearer Token puro, o "credentials: include" é opcional, 
        // mas não atrapalha se o CORS estiver configurado.
        body: JSON.stringify({ username, password })
      });

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        throw new Error(errorData.message || "Usuário ou senha inválidos");
      }

      const data = await response.json();
      
      // O nome da propriedade depende da sua API (pode ser data.token ou data.accessToken)
      if (data.token) {
        localStorage.setItem('token', data.token);
        setIsLoggedIn(true);
        alert("Login realizado com sucesso!");
      } else {
        throw new Error("Token não recebido do servidor.");
      }

    } catch (err) {
      setError(err.message);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    setIsLoggedIn(false);
  };

  if (isLoggedIn) {
    return (
      <div>
        <h2>Você está logado!</h2>
        <p>Seu Token: {localStorage.getItem('token')?.substring(0, 20)}...</p>
        <button onClick={handleLogout}>Sair</button>
      </div>
    );
  }

  return (
    <div>
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <input 
          type="text" 
          placeholder="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
        /><br />

        <input 
          type="password" 
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        /><br />

        <button type="submit">Entrar</button>
      </form>
      {error && <p style={{ color: "red" }}>{error}</p>}
    </div>
  );
}

export default Login;
