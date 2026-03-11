import React, { useState, useRef, useEffect, useCallback } from 'react';
import api, { baseURL } from './Api';

export function MediaPlayer() {
  const [videos, setVideos] = useState([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  // Controle do "gate" de interação
  const [userActivated, setUserActivated] = useState(false);
  const [isFullscreen, setIsFullscreen] = useState(false);

  const videoRef = useRef(null);

  // ---- Util: request de fullscreen com todos os prefixes ----
  const requestFsOn = useCallback(async (el) => {
    if (!el) return;
    const req =
      el.requestFullscreen ||
      el.webkitRequestFullscreen ||
      el.mozRequestFullScreen ||
      el.msRequestFullscreen;

    if (req) {
      // Alguns browsers precisam de call simples, outros retornam promise
      const ret = req.call(el);
      if (ret && typeof ret.catch === 'function') {
        await ret;
      }
      return;
    }

    // iOS Safari – fullscreen "nativo" do elemento de vídeo
    if (el.webkitEnterFullscreen) {
      el.webkitEnterFullscreen();
      return;
    }

    throw new Error('Fullscreen API não suportada');
  }, []);

  // ---- Util: sair do fullscreen (se precisar) ----
  const exitFs = useCallback(async () => {
    const doc = document;
    const exit =
      doc.exitFullscreen ||
      doc.webkitExitFullscreen ||
      doc.mozCancelFullScreen ||
      doc.msExitFullscreen;

    if (exit) {
      const ret = exit.call(doc);
      if (ret && typeof ret.catch === 'function') {
        await ret;
      }
    }
  }, []);

  // ---- Buscar lista de vídeos ----
  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) { setLoading(false); return; }

    let active = true;
    api.get('/api/MediaFile/ListActiveMediaFiles')
      .then((response) => {
        if (!active) return;
        const dados = Array.isArray(response.data) ? response.data : [];
        const lista = dados.map(item => ({
          title: item.title,
          description: item.description,
          fileName: item.fileName
        }));
        setVideos(lista);
        setLoading(false);
      })
      .catch((err) => {
        console.error('Erro ao buscar vídeos:', err);
        if (!active) return;
        setError('Não foi possível carregar a lista de vídeos.');
        setLoading(false);
      });

    return () => { active = false; };
  }, []);

  // ---- Autoplay ao trocar de vídeo (já em fullscreen ou não) ----
  useEffect(() => {
    if (!videoRef.current || videos.length === 0) return;

    const v = videoRef.current;
    v.load();

    // Políticas de autoplay exigem muted (já está na prop)
    const playPromise = v.play();
    if (playPromise && typeof playPromise.catch === 'function') {
      playPromise.catch(err => {
        console.warn('Autoplay bloqueado (tente após interação do usuário):', err);
      });
    }
  }, [currentIndex, videos]);

  // ---- Ao terminar um vídeo, avança (ou reinicia) ----
  const handleEnded = () => {
    if (videos.length === 0) return;

    if (videos.length === 1) {
      videoRef.current.currentTime = 0;
      videoRef.current.play();
    } else {
      setCurrentIndex(prev => (prev + 1) % videos.length);
    }
  };

  // ---- Listener do estado de fullscreen ----
  useEffect(() => {
    const handleFsChange = () => {
      const doc = document;
      const fsElement =
        doc.fullscreenElement ||
        doc.webkitFullscreenElement ||
        doc.mozFullScreenElement ||
        doc.msFullscreenElement;
      setIsFullscreen(!!fsElement);
    };

    document.addEventListener('fullscreenchange', handleFsChange);
    document.addEventListener('webkitfullscreenchange', handleFsChange);
    document.addEventListener('mozfullscreenchange', handleFsChange);
    document.addEventListener('MSFullscreenChange', handleFsChange);

    return () => {
      document.removeEventListener('fullscreenchange', handleFsChange);
      document.removeEventListener('webkitfullscreenchange', handleFsChange);
      document.removeEventListener('mozfullscreenchange', handleFsChange);
      document.removeEventListener('MSFullscreenChange', handleFsChange);
    };
  }, []);

  // ---- Gesto inicial: entrar em fullscreen + dar play ----
  const startFullscreenPlayback = async () => {
    try {
      const v = videoRef.current;
      if (!v) return;

      // 1) Solicita fullscreen no container (melhor UX) ou no próprio vídeo
      const container = v.parentElement; // wrapper
      try {
        await requestFsOn(container || v);
      } catch {
        // Fallback: tentar no <video> se o container falhar
        await requestFsOn(v);
      }

      setUserActivated(true);

      // 2) Play (agora a policy permite)
      await v.play();

      // iOS tip: trava rotação em paisagem se desejar (Screen Orientation API, quando disponível)
      // if (screen.orientation && screen.orientation.lock) {
      //   try { await screen.orientation.lock('landscape'); } catch {}
      // }
    } catch (err) {
      console.warn('Não foi possível iniciar em fullscreen:', err);
      // Mesmo se fullscreen falhar, marque como ativado para permitir play
      setUserActivated(true);
      // Último recurso: tentar só o play
      try { await videoRef.current?.play(); } catch {}
    }
  };

  // ---- Render guards ----
  if (loading) return <p>Carregando mídias...</p>;
  if (error) return <p style={{ color: 'red' }}>{error}</p>;
  const token = localStorage.getItem('token');
  if (!token) return <p>Aguardando login...</p>;
  if (videos.length === 0) return <p>Nenhum vídeo disponível no momento.</p>;

  const currentVideo = videos[currentIndex];
  const currentVideoUrl = `${baseURL}/Videos/${currentVideo.fileName}?token=${token}`;

  return (
    <div className="container">
      <div
        className="video-wrapper"
      >
        {/* Gate de interação: só aparece antes do primeiro gesto */}
        {!userActivated && (
          <button
            onClick={startFullscreenPlayback}
            style={{
              position: 'absolute',
              inset: 0,
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              background: 'rgba(0,0,0,0.7)',
              color: '#fff',
              border: 'none',
              fontSize: 22,
              cursor: 'pointer',
              zIndex: 2,
              borderRadius: 8
            }}
            aria-label="Iniciar em tela cheia"
          >
            Clique para iniciar em tela cheia
          </button>
        )}

        <video
          ref={videoRef}
          className="video-player"
          style={{ borderRadius: 8, boxShadow: '0 4px 8px rgba(0,0,0,0.2)' }}
          autoPlay
          muted   // necessário para autoplay
          playsInline // importante no iOS para não forçar o player nativo
          onEnded={handleEnded}
          // Não use onPlay para fullscreen: será bloqueado
        >
          <source src={currentVideoUrl} type="video/mp4" />
          Seu navegador não suporta o elemento de vídeo.
        </video>

        {/* Opcional: badge simples com status */}
        <div style={{ marginTop: 8, fontSize: 12, color: '#666' }}>
          Vídeo {currentIndex + 1} de {videos.length}
          {isFullscreen ? ' — tela cheia' : ''}
        </div>
      </div>
    </div>
  );
}