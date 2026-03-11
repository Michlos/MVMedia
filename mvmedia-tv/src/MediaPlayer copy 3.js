import React, { useState, useRef, useEffect, useCallback } from 'react';
import api from './Api'; // Importa a instância do axios configurada
import { baseURL } from './Api';

export function MediaPlayer() {
  const [videos, setVideos] = useState([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [loading, setLoading] = useState(true); // Corrigido de leading
  const [error, setError] = useState(null);
  

  //controle do "gate" de interação
  const [userActivated, setUserActivated] = useState(false);
  const [isFullscreen, setIsFullscreen] = useState(false);

  const videoRef = useRef(null);


  // Busca a lista de vídeos
  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) {
      setLoading(false);
      return;
    }

    api.get('/api/MediaFile/ListActiveMediaFiles')
      .then(response => {
        // Verifica se a resposta é um array
        const dados = Array.isArray(response.data) ? response.data : [];
        const lista = dados.map(item => ({
          title: item.title,
          description: item.description,
          fileName: item.fileName
        }));
        
        setVideos(lista);
        setLoading(false);
      })
      .catch(err => {
        console.error('Erro ao buscar vídeos:', err);
        setError("Não foi possível carregar a lista de vídeos.");
        setLoading(false);
      });
  }, []);

  // Lógica de autoplay quando o currentIndex muda
  useEffect(() => {
    if (!videoRef.current || videos.length === 0) return;
    
    videoRef.current.load(); // Importante para atualizar o <source>
    const playPromise = videoRef.current.play();

    if (playPromise !== undefined) {
      playPromise.catch(err => console.warn('Autoplay bloqueado:', err));
    }
  }, [currentIndex, videos]);

  const handleEnded = () => {
    if (videos.length === 0) return;

    if(videos.length === 1) {
      videoRef.current.currentTime = 0;
      videoRef.current.play();
    } else{
      // Pula para o próximo vídeo ou volta ao primeiro
      setCurrentIndex(prev => (prev + 1) % videos.length);
    }
  };

  // 1. Verificação de Loading correta
  if (loading) return <p>Carregando mídias...</p>;
  
  // 2. Verificação de erro
  if (error) return <p style={{color: 'red'}}>{error}</p>;
  
  // 3. Se não estiver logado ou sem vídeos
  if (!localStorage.getItem('token')) return <p>Aguardando login...</p>;
  if (videos.length === 0) return <p>Nenhum vídeo disponível no momento.</p>;

  const currentVideo = videos[currentIndex];
  const token = localStorage.getItem('token');
  
  // URL formatada com o token para que o servidor libere o arquivo .mp4
  const currentVideoUrl = `${baseURL}/Videos/${currentVideo.fileName}?token=${token}`;



  return (
    <div className="container" style={{ textAlign: 'center', marginTop: '20px' }}>
      <div className="video-wrapper">
        
        <video
          ref={videoRef}
          className="video-player"
          style={{ maxWidth: '100%', borderRadius: '8px', boxShadow: '0 4px 8px rgba(0,0,0,0.2)' }}
          autoPlay
          muted // Muted é obrigatório para autoplay funcionar na maioria dos browsers
          playsInline
          onEnded={handleEnded}
        >
          <source src={currentVideoUrl} type="video/mp4" />
          Seu navegador não suporta o elemento de vídeo.
        </video>
      </div>
    </div>
  );
}
