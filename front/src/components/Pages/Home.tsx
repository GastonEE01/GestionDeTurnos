import  {   useState } from 'react'

// Componentes
import { LocalesTable } from '../Locales/LocalesTable'
import type { LocalesType } from '../Locales/LocalesType'
import { Filter } from '../UIX/Filter';
import { SearchText } from '../UIX/SearchText.tsx';
// Hook
import { useDebouce } from '../../hook/useDebouce.ts';
const LOCALES_DE_PRUEBA: LocalesType[] = [
  {
    Id: 1,
    Nombre: "Barbería Centro",
    Descripcion: "Cortes clásicos y modernos",
    Categoria: "Estética",
    ImagenURL: "https://placeholder.com",
    Direccion: "Av. Corrientes 1234",
    Telefono: "1122334455"
  },
  {
    Id: 2,
    Nombre: "Consultorio Norte",
    Descripcion: "Atención médica general",
    Categoria: "Salud",
    ImagenURL: "https://placeholder.com",
    Direccion: "Calle Mitre 567",
    Telefono: "1199887766"
  }
];

export const Home = () => {

    // Estados
    const [searchText,setSearchText] = useState('')
    const [filterCategoria,setFilterCategoria] = useState('')

    const debouseText = useDebouce(searchText,500)
    const localFilter = LOCALES_DE_PRUEBA.filter((local) => {
       const coincideNombre =  local.Nombre.toLocaleLowerCase().includes(debouseText.toLocaleLowerCase());
       const coincideCategoria = filterCategoria === "" || local.Categoria === filterCategoria;
       return coincideCategoria && coincideNombre;
    });


    console.log("Texto: " ,debouseText)

   /*const handleEditar = (id: number) => {
    console.log("Editando local: ", id);
   }*/

    return (
    <div>
      <h1>Home</h1>
      <div>
      <SearchText value={searchText} onChange={(e) => setSearchText(e.target.value)} />
       <Filter value={filterCategoria} onChange={(e) => setFilterCategoria(e.target.value)} />    
      </div>
      <LocalesTable data={localFilter}  />      
    </div>
  )
}

