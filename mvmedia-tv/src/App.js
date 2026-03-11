import logo from './logo.svg';
import './App.css';
import React, { useState, useEffect } from 'react';
import Login from './Login';
// 1. Importe o componente externo (certifique-se que o caminho está correto)
import { MediaPlayer } from './MediaPlayer'; 

function App() {
  // Estado para controlar se o usuário está logado
  const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem('token'));

  // Efeito para monitorar o localStorage (opcional, melhora a experiência)
  useEffect(() => {
    
    //TRATA ERRO DE "bROKEN pIPE" (epipe) COMUNS EM TVS
    const handleGlogalError = (error) => {
      if(error && error.message && error.message.includes('EPIPE')){
        return; //IGNORA SILENCIOSAMENTE E NÃO TRAVA A TV
      }
    };

    window.addEventListener('error', handleGlogalError);
    window.addEventListener('unhandledrejection', handleGlogalError);

    //se estiver usando eletroni, tenta acessar o process global com segurança
    if(window.process){
      window.process.on('uncaughtException', handleGlogalError);
    }

    const checkToken = () => {
      setIsLoggedIn(!!localStorage.getItem('token'));
    };
    
    // Escuta mudanças no storage para atualizar o App se o usuário deslogar/logar
    window.addEventListener('storage', checkToken);
    return () => window.removeEventListener('storage', checkToken);
  }, []);

  return (
    <div className="App">
      <header className="App-header">
        
        {/* 2. Mostra o Login sempre, ou você pode fazer um condicional aqui também */}
        <Login />

        

        {/* 3. Lógica condicional: Só renderiza o MediaPlayer se houver um token */}
        {isLoggedIn ? (
          <div style={{ width: '100%' }}>
            <MediaPlayer />
          </div>
        ) : (
          <p>Por favor, faça login para visualizar os vídeos.</p>
        )}

       
      </header>
    </div>
  );
}

export default App;
