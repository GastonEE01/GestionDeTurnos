import  {   useEffect, useState } from 'react'
import { getLocales } from '../../service/api.ts';
// Componentes
import { LocalesTable } from '../Locales/LocalesTable'
import type { LocalesType } from '../Locales/LocalesType'
import type { ServicioType } from '../Servicio/ServicioType.ts'
import { Filter } from '../UIX/Filter';
import { SearchText } from '../UIX/SearchText.tsx';
// Hook
import { useDebouce } from '../../hook/useDebouce.ts';

/*const LOCALES_DE_PRUEBA: LocalesType[] = [
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
];*/

export const Home = () => {

    // Estados
    const [searchText,setSearchText] = useState('')
    const [filterCategoria,setFilterCategoria] = useState('')
    const [locales, setLocales] = useState<LocalesType[]>([]);
    useEffect(() => {
      const loadingLocales = async () => {
        try{
          const data = await getLocales();
          setLocales(data);
        } catch (error){
          console.error("Error al traer locales: ", error);
        }
      };
      loadingLocales()
    },[]);

    const debouseText = useDebouce(searchText,500)
    const localFilter = locales.filter((locales) => {
       const nameEquals =  locales.name.toLocaleLowerCase().includes(debouseText.toLocaleLowerCase());
       const categoryEquals = filterCategoria === "" || locales.category === filterCategoria;
       return categoryEquals && nameEquals;
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

