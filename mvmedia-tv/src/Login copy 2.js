import api from './Api';

const handleLogin = async (credentials) => {
  try {
    const response = await api.post('/login', credentials);
    const { token } = response.data;
    
    // Salva o token para uso futuro
    localStorage.setItem('token', token);
    
    // Opcional: Redirecionar usuário
  } catch (error) {
    console.error('Erro no login', error);
  }
};

export default handleLogin;
