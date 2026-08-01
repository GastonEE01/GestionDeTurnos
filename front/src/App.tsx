import './App.css'
import {  Route, Routes } from 'react-router-dom'

// Componentes
import { Header } from './components/Header/Header'
import { Home } from './components/Pages/Home'
import { Login } from './components/Pages/Login' 
import { Registro } from './components/Pages/Registro'

function App() {
 

  return (
    <>
     <div>
      <Routes>
        <Route path='/Home' element={     
        <>
        <Header/>
        <Home/>
        </>}>
        </Route>
        
        <Route path='/' element={     
        <>
        <Login />
        </>}>
        </Route>

        
        <Route path='/Registro' element={     
        <>
        <Registro />
        </>}>
        </Route>
       </Routes>
     </div>
    </>
  )
}

export default App
