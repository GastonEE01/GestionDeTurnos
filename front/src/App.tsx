import './App.css'
import {  Route, Routes } from 'react-router-dom'

// Componentes
import { Header } from './components/Header/Header'
import { Home } from './components/Pages/Home'

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
        <Header/>
        <Home/>
        </>}>
        </Route>
        
        

        
        
       </Routes>
     </div>
    </>
  )
}

export default App
