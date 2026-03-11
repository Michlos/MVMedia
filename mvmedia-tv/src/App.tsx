import React, { useEffect, useState } from "react";
import "./App.css";

import getMedias, { Media } from "./services/get-medias";
import MediaComponent from "./components/media/media.component";

function App() {
  const [medias, setMedias] = useState<Media[]>([]);

    useEffect(() => {
        const fetchData = async () => {
              try {
                      const data = await getMedias();
                              setMedias(data);
                                    } catch (error) {
                                            console.error("Erro ao buscar mídias:", error);
                                                  }
                                                      };

                                                          fetchData();
                                                            }, []);

                                                              return (
                                                                  <div className="App">
                                                                        <header className="App-header">
                                                                                <p>Rodando na TV</p>
                                                                                      </header>

                                                                                            <main>
                                                                                                    {medias.length === 0 ? (
                                                                                                              <p>Carregando mídias...</p>
                                                                                                                      ) : (
                                                                                                                                medias.map((media) => (
                                                                                                                                            <MediaComponent key={media.FileName} media={media} />
                                                                                                                                                      ))
                                                                                                                                                              )}
                                                                                                                                                                    </main>
                                                                                                                                                                        </div>
                                                                                                                                                                          );
                                                                                                                                                                          }

                                                                                                                                                                          export default App;