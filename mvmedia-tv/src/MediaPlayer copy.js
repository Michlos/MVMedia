import React, { useState, useRef, useEffect } from 'react';
import { API_BASE_URL } from './App';

// Componente que busca os vídeos e toca em sequência
export function MediaPlayer() {
  const [videos, setVideos] = useState([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [fullscreenRequested, setFullscreenRequested] = useState(false);

  const videoRef = useRef(null);

  useEffect(() => {
    fetch(`${API_BASE_URL}/api/MediaFile/ListActiveMediaFiles`, {
      method: "GET",
      credentials: "include"   // <<=== ESSENCIAL PARA O CAMINHO A
    })
      .then(response => {
        if (!response.ok) {
          throw new Error(`Erro HTTP: ${response.status}`);
        }
        return response.json();
      })
      .then(json => {
        const lista = json.map(item => ({
          title: item.title,
          description: item.description,
          fileName: item.fileName
        }));

        setVideos(lista);
        setLoading(false);
      })
      .catch(error => {
        console.error('Erro:', error);
        setError(error.message);
        setLoading(false);
      });
  }, []);

  const tryFullscreen = () => {
    const player = videoRef.current;
    if (!player) return;

    if (document.fullscreenElement ||
      document.webkitFullscreenElement ||
      document.msFullscreenElement) {
      return;
    }

    // fullscreen automático está desativado porque causa erros no Edge
  };

  const handleCanPlay = () => {
    if (!fullscreenRequested) {
      tryFullscreen();
      setFullscreenRequested(true);
    }
  };

  const handlePlay = () => {
    tryFullscreen();
  };

  const handleEnded = () => {
    if (videos.length === 0) return;
    setCurrentIndex(prev => (prev + 1) % videos.length);
    setFullscreenRequested(false);
  };

  useEffect(() => {
    if (!videoRef.current || videos.length === 0) return;
    videoRef.current.load();

    videoRef.current
      .play()
      .catch(err => console.warn('Não foi possível dar play automático:', err));
  }, [currentIndex, videos]);

  if (loading) return <p>Carregando mídias...</p>;
  if (error) return <p>Erro ao carregar mídias: {error}</p>;
  if (videos.length === 0) return <p>Nenhum vídeo disponível.</p>;

  const currentVideo = videos[currentIndex];
  const currentVideoUrl = `${API_BASE_URL}/Videos/${currentVideo.fileName}`;

  return (
    <div className="container">
      <div className="video-wrapper">
        <video
          id="mainVideoPlayer"
          ref={videoRef}
          className="video-player"
          style={{ maxWidth: '800px', maxHeight: '500px' }}
          controls
          autoPlay
          muted
          onCanPlay={handleCanPlay}
          onPlay={handlePlay}
          onEnded={handleEnded}
        >
          <source src={currentVideoUrl} type="video/mp4" />
          Seu navegador não suporta o elemento de vídeo.
        </video>

        <div className="mt-3">
          <span id="videoTitle" className="fw-bold">
            {currentVideo.title}
          </span>
          <span id="videoDescription" className="ms-2">
            {currentVideo.description}
          </span>
        </div>
      </div>
    </div>
  );
}