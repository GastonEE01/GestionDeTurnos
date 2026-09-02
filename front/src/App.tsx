import './App.css'
import {  Route, Routes } from 'react-router-dom'

// Componentes
import { Home } from './components/Pages/Home'
import { ProtectedRoute } from './components/ProtectedRoute'

// Páginas
import { Landing } from './components/Pages/Landing' 
import { Login } from './components/Pages/Login' 
import { RegisterClient } from './components/Pages/RegisterClient'
import { RegisterLocal } from './components/Pages/RegisterLocal'

function App() {
 

  return (
    <>
     <div>
      <Routes>
        <Route path='/' element={     
        <>
        <Landing />
        </>}>
        </Route>

        <Route path='/registro/cliente' element={     
        <>
        <RegisterClient />
        </>}>
        </Route>

        <Route path='/registro/local' element={     
        <>
        <RegisterLocal />
        </>}>
        </Route>

        <Route path='/iniciar-sesion' element={     
        <>
        <Login />
        </>}>
        </Route>

        <Route path='/Inicio' element={     
        <>
        <Home/>
        </>}>
        </Route>
        
        <Route element={<ProtectedRoute />}>
          <Route path="/Inicio" element={<Home />} />
        </Route>
        
       </Routes>
     </div>
    </>
  )
}

export default App
