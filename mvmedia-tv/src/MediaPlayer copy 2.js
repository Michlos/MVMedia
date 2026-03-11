import React, {useState, useRef, useEffect} from 'react';
import api from './Api'; // Importa a instância do axios configurada
import { baseURL } from './Api';

export function MediaPlayer(){
  const [videos, setVideos] = useState([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [leading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [fullscreddnRequested, setFullScreenRequest] = useState(false);

  const videoRef = useRef(null);

  // Busca a lista de vídeos usando a instância do Axios
  useEffect(() => {
    api.get('/api/MediaFile/ListActiveMediaFiles')
      .then(response => {
        const lista = response.data.map(item => ({
          title: item.title,
          description: item.description,
          fileName: item.fileName
        }));
        setVideos(lista);
        setLoading(false);
      })
      .catch(err => {
        console.error('Erro ao buscar vídeos:', err);
        setError(err.message);
        setLoading(false);
      });
  }, []);

  // Lógica de troca de vídeo e autoplay
  useEffect(() => {
    if (!videoRef.current || videos.length === 0) return;
    
    videoRef.current.load();
    videoRef.current
      .play()
      .catch(err => console.warn('Autoplay bloqueado pelo navegador:', err));
  }, [currentIndex, videos]);

  const tryFullscreen = () => {
    const player = videoRef.current;
    if (!player || document.fullscreenElement) return;
    // Lógica opcional de fullscreen aqui
  };

  const handleCanPlay = () => {
    if (!fullscreddnRequested) {
      tryFullscreen();
      setFullScreenRequest(true);
    }
  };

  const handleEnded = () => {
    if (videos.length === 0) return;
    setCurrentIndex(prev => (prev + 1) % videos.length);
    setFullScreenRequest(false);
  };

  if (setLoading) return <p>Carregando mídias...</p>;
  if (error) return <p>Erro ao carregar mídias: {error}</p>;
  if (videos.length === 0) return <p>Nenhum vídeo disponível.</p>;

  const currentVideo = videos[currentIndex];

  /**
   * IMPORTANTE: A tag <video> não envia o Bearer Token nos headers.
   * Se a sua rota de arquivos estáticos (/Videos/...) for protegida, 
   * você deve passar o token via Query String como abaixo:
   */
  const token = localStorage.getItem('token');
  const currentVideoUrl = `${baseURL}/Videos/${currentVideo.fileName}?token=${token}`;

  return (
    <div className="container">
      <div className="video-wrapper">
        <video
          id="mainVideoPlayer"
          ref={videoRef}
          className="video-player"
          style={{ maxWidth: '800px', maxHeight: '500px', width: '100%' }}
          controls
          autoPlay
          muted
          onCanPlay={handleCanPlay}
          onEnded={handleEnded}
        >
          <source src={currentVideoUrl} type="video/mp4" />
          Seu navegador não suporta o elemento de vídeo.
        </video>

        <div className="mt-3">
          <h5 id="videoTitle" className="fw-bold d-inline">
            {currentVideo.title}
          </h5>
          <p id="videoDescription" className="ms-2 d-inline">
            {currentVideo.description}
          </p>
        </div>
      </div>
    </div>
  );
}