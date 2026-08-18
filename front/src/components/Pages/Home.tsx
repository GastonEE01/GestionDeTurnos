import { getLocales } from '../../service/api.ts';
import { getLocalesByUser } from '../../service/api.ts';

import { useNavigate } from 'react-router-dom';
import  { useEffect, useState } from 'react'

// componentes
import { LocalesTable } from '../Locales/LocalesTable'
import { Filter } from '../UIX/Filter';
import { SearchText } from '../UIX/SearchText.tsx';
// interface
import type { LocalesType } from '../../interface/LocalesType'
//import type {LoginResponse} from '../../interface/LoginType.ts'

// Hook
import { useDebouce } from '../../hook/useDebouce.ts';
import { useAuthStore } from "../../hook/useAuthStore";

export const Home = () => {

    // Estados
    const navigate = useNavigate();
    
    // Consumimos al usuario y la funcion de logout 
    const user = useAuthStore((state) => state.user);
    const logout = useAuthStore((state) => state.logout);

    const [searchText,setSearchText] = useState('')
    const [filterCategoria,setFilterCategoria] = useState('')
    const [locales, setLocales] = useState<LocalesType[]>([]);
    
   useEffect(() => {
        if (!user) {
            navigate("/");
        }
    }, [user, navigate]);

    // Mostrar los locales segun el rol
    useEffect(() => {
      if(!user) return;// esto evital el "null""
      const loadingLocales = async () => {
        try{
          if(user.rol === "Local"){
          const data = await getLocalesByUser(user.id);
          setLocales(data); 
          } else{
            const data = await getLocales();
            setLocales(data); 
          }
        } catch(error){
          console.error("Error al traer locales: ", error);
        }
      };
      loadingLocales();
      },[user]);
 
    // Obtener todos los locales
    /*useEffect(() => {
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

    // Obtener todos los locales de cada usuario local
    useEffect(() => {
      const loadingLocalesUsers = async () => {
        try{
          const data = await getLocalesByUser(user.id);
          setLocales(data);
        } catch(error){
          console.error("Error al traer los locales de cada usuario: ", error)
        }
      };
      loadingLocalesUsers()
    },[]);*/

    // Custom Hooks y lógica computada
    const debouseText = useDebouce(searchText,500)
    const localFilter = locales.filter((locales) => {
       const nameEquals =  locales.name.toLocaleLowerCase().includes(debouseText.toLocaleLowerCase());
       const categoryEquals = filterCategoria === "" || locales.category === filterCategoria;
       return categoryEquals && nameEquals;
    });

    const handleLogout = () => {
        logout();
        navigate("/");
    };
  if (!user) {
        return <h2>Cargando datos del usuario...</h2>;
    }
    return (
    <div>
      <h1>Home</h1>
      <div>
        <h1>¡Bienvenido a la plataforma, {user.name}! 👋</h1>
      <p>Tu correo electrónico registrado es: <strong>{user.email}</strong></p>
      <p>Tu rol actual en el sistema es: <span style={{ textTransform: "uppercase" }}>{user.rol}</span></p>
        <button onClick={handleLogout} style={{ marginTop: "20px", color: "red" }}>
        Cerrar Sesión
      </button>
      </div>

      {/* Vista cliente */}
      {user.rol === "Cliente" && (
        <div style={{ marginTop: "20px" }}>
          <h2>Catálogo de Locales</h2>
          <SearchText value={searchText} onChange={(e) => setSearchText(e.target.value)} />
          <Filter value={filterCategoria} onChange={(e) => setFilterCategoria(e.target.value)} />
          <LocalesTable data={localFilter} user={user} />
        </div>
      )}

        {/* Vista local*/}
        {user.rol === "Local" && (
          <div style={{ marginTop: "20px" }}>
          <h2>Panel de Administración de tu Local</h2>
          <p>Acá se mostrará la agenda de turnos recibidos y edición de horarios.</p>
          <LocalesTable data={localFilter} user={user} />
        </div>
        )}
    </div>
  );
};

