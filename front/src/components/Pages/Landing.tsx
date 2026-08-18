//import React from 'react'
import { useNavigate } from 'react-router-dom'

export const Landing = () => {
    const navigate = useNavigate();

  return (
    <div>
      <h1>Bienvenido a la aplicación de turnos</h1>
      <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ipsum maiores quos est repudiandae quaerat voluptatem, minima iste, fugit sapiente dolorum quam laboriosam accusantium dolores illo quia magnam quidem dicta vero?</p>

      <button onClick={() => navigate('/registro/cliente')}>Registrarse como Cliente</button>
      <button onClick={() => navigate('/registro/local')}>Registrarse como Local</button>
    </div>

    
  )
}

