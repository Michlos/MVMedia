import logo from './logo.svg';
import './App.css';
import React, { useState, useEffect } from 'react';

function DataFetcher() {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    // >>> Ajuste da URL da sua API <<<
    fetch('http://localhost:5069/api/MediaFile/ListActiveMediaFiles')
      .then(response => {
        if (!response.ok) {
          throw new Error(`Erro HTTP: ${response.status}`);
        }
        return response.json();
      })
      .then(json => {
        // json deve ser um array igual ao response que você mostrou
        // Aqui filtramos só os campos que você quer
        const somenteCamposNecessarios = json.map(item => ({
          title: item.title,
          description: item.description,
          fileName: item.fileName
        }));

        setData(somenteCamposNecessarios);
        setLoading(false);
      })
      .catch(error => {
        console.error('Erro:', error);
        setError(error.message);
        setLoading(false);
      });
  }, []);

  if (loading) return <p>Carregando mídias...</p>;
  if (error) return <p>Erro ao carregar mídias: {error}</p>;

  return (
    <div>
      <h2>Lista de Mídias Ativas</h2>
      <ul style={{ textAlign: 'left', maxWidth: '600px' }}>
        {data.map(item => (
          <li key={item.fileName} style={{ marginBottom: '16px' }}>
            <strong>Título:</strong> {item.title}<br />
            <strong>Descrição:</strong> {item.description}<br />
            <strong>Arquivo:</strong> {item.fileName}
          </li>
        ))}
      </ul>
    </div>
  );
}

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          TESTE DE SITE DO GERADOR DE MÍDIA DO MIKE E DO VITOR
        </p>

        {/* Aqui você usa o DataFetcher para mostrar as mídias da API */}
        <DataFetcher />

        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default App;
