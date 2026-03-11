import React, { useState, useEffect } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'

import getMedias, { Media } from "./services/get-medias";
import MediaComponent from "./components/media/media.component";


function App() {

  const [medias, setMedias] = useState<Media[]>([]);
  
    useEffect(() => {
        const fetchData = async () => {
          try{
            const data = await getMedias();
            setMedias(data);
          }
          catch(error){
            console.error("Error fetching medias:", error);
          }
        };
        fetchData();
    }, []);

  const [count, setCount] = useState(0)
  return (
    <>
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
      </div>
      <div>
        {medias.length === 0 ? (
          <p>Loading medias...</p>
        ) : (
          medias.map((media) => (
            <MediaComponent key={media.FileName} media={media} />
          ))
        )}
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  )
}

export default App
