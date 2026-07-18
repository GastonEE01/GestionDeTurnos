import './App.css'
import {  Route, Routes } from 'react-router-dom'

// Componentes
import { Header } from './components/Header/Header'
import { Home } from './components/Pages/Home'

function App() {
 

  return (
    <>
     <div>
      <Routes>
        <Route path='/Home' element={ 
        <>
        <Header/>
        <Home/>
        
        </>}></Route>
      </Routes>
     </div>
    </>
  )
}

export default App
