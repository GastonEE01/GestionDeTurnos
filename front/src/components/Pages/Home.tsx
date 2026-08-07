import  {    useEffect, useState } from 'react'
import { getLocales } from '../../service/api.ts';
// Componentes
import { LocalesTable } from '../Locales/LocalesTable'
import type { LocalesType } from '../Locales/LocalesType'
import type {LoginResponse} from '../../service/api.ts'
//import type { ServicioType } from '../Servicio/ServicioType.ts'
import { Filter } from '../UIX/Filter';
import { SearchText } from '../UIX/SearchText.tsx';
// Hook
import { useDebouce } from '../../hook/useDebouce.ts';
import { useNavigate } from 'react-router-dom';


export const Home = () => {

    // Estados
    const navigate = useNavigate();
    
    // 1️⃣ GRUPO DE HOOKS: Todos bien arriba juntos
    const [userData, setUserData] = useState<LoginResponse | null>(() => {
        const user = localStorage.getItem("user");
        const token = localStorage.getItem("token");
        if (!user || !token) return null;

        try {
         // const parsed = JSON.parse(user);//aca
         return JSON.parse(user) as LoginResponse;
             } catch(error) {
              console.error("Error al parsear el usuario del localStorage:", error);
            return null;
        }
    });

    const [searchText,setSearchText] = useState('')
    const [filterCategoria,setFilterCategoria] = useState('')
    const [locales, setLocales] = useState<LocalesType[]>([]);
    
   useEffect(() => {
        if (!userData) {
            navigate("/");
        }
    }, [userData, navigate]);

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

    // Custom Hooks y lógica computada
    const debouseText = useDebouce(searchText,500)
    const localFilter = locales.filter((locales) => {
       const nameEquals =  locales.name.toLocaleLowerCase().includes(debouseText.toLocaleLowerCase());
       const categoryEquals = filterCategoria === "" || locales.category === filterCategoria;
       return categoryEquals && nameEquals;
    });

    const handleLogout = () => {
        localStorage.clear();
        navigate("/");
    };
    console.log("Texto: " ,debouseText)

  // 2️⃣ GRUPO DE RETURNS CONDICIONALES: Ahora sí van abajo de los hooks
  if (!userData) {
        return <h2>Cargando datos del usuario...</h2>;
    }

    return (
    <div>
      <h1>Home</h1>
      <div>
        <h1>¡Bienvenido a la plataforma, {userData.name}! 👋</h1>
      <p>Tu correo electrónico registrado es: <strong>{userData.email}</strong></p>
      <p>Tu rol actual en el sistema es: <span style={{ textTransform: "uppercase" }}>{userData.rol}</span></p>
        <button onClick={handleLogout} style={{ marginTop: "20px", color: "red" }}>
        Cerrar Sesión
      </button>

      <SearchText value={searchText} onChange={(e) => setSearchText(e.target.value)} />
       <Filter value={filterCategoria} onChange={(e) => setFilterCategoria(e.target.value)} />    
      </div>
      <LocalesTable data={localFilter}  usuarioId={userData.id} />      
    </div>
  )
}

