import { getLocales } from '../../service/api.ts';
import { getLocalesByUser } from '../../service/api.ts';

import { useNavigate } from 'react-router-dom';
import  { useEffect, useState } from 'react'
import { ArrowRight, CalendarDays, Sparkles, Search, Users } from 'lucide-react'

// componentes
import { LocalesTable } from '../Locales/LocalesTable'
import { Filter } from '../UIX/Filter';
import { SearchText } from '../UIX/SearchText.tsx';
import { TurnosTable } from '../Turnos/TurnosTable.tsx'

// interface
import type { LocalesType } from '../../interface/LocalesType'

// Hook
import { useDebouce } from '../../hook/useDebouce.ts';
import { useAuthStore } from "../../hook/useAuthStore";

export const Home = () => {

    // Estados
    const user = useAuthStore((state) => state.user);
    const logout = useAuthStore((state) => state.logout);
    const navigate = useNavigate();
    const [section, setSection] = useState('Inicio');
    const [searchText,setSearchText] = useState('')
    const [filterCategoria,setFilterCategoria] = useState('')
    const [locales, setLocales] = useState<LocalesType[]>([]);
    
    // Opciones de navegación dinámicas según el rol
  const clientNav = ['Inicio', 'Pedir turno', 'Mis turnos', 'Historial'];
  const ownerNav = ['Inicio', 'Mis locales', 'Servicios', 'Horarios'];
  const navItems = user?.rol === 'Local' ? ownerNav : clientNav;

   useEffect(() => {
        if (!user) {
            navigate("/");
        }
    }, [user, navigate]);

    // Mostrar los locales segun el rol
    useEffect(() => {
      if(!user) return;
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
        return (
        <div className="flex min-h-screen items-center justify-center bg-gray-50">
        <h2 className="text-lg font-semibold text-gray-600">Cargando datos del usuario...</h2>
      </div>
    );
    }

    return (
    <div className="min-h-screen bg-gray-50 text-gray-900">
      <header className="border-b border-gray-200 bg-white px-6 py-4">
    <div className="mx-auto flex max-w-7xl items-center justify-between">
      {/* Logo */}
      <div className="flex items-center gap-3">
        <div className="flex size-10 items-center justify-center rounded-xl bg-black font-bold text-white">
          T
        </div>
        <span className="text-xl font-bold tracking-tight">turnito</span>
      </div>

      {/* Switcher de Rol (Cliente / Local) */}
      <div className="hidden items-center gap-2 rounded-full border border-gray-200 bg-gray-100 p-1 md:flex">
        <span className="rounded-full bg-white px-4 py-1.5 text-xs font-semibold shadow-sm">
          {user.rol}
        </span>
      </div>

      {/* Perfil de usuario y Cierre de sesión */}
      <div className="flex items-center gap-4">
        <div className="text-right hidden sm:block">
          <p className="text-sm font-semibold text-gray-800">{user.name}</p>
          <p className="text-xs text-gray-500">{user.email}</p>
        </div>
        <button
          onClick={handleLogout}
          className="rounded-xl border border-red-200 bg-red-50 px-4 py-2 text-sm font-semibold text-red-600 transition-colors hover:bg-red-100"
        >
          Cerrar Sesión
        </button>
      </div>
    </div>
  </header>
      
   <div className="flex w-full flex-col lg:flex-row">
    {/* BARRA LATERAL (SIDEBAR) */}
    <aside className="border-b border-gray-200 bg-white px-5 py-4 lg:min-h-[calc(100vh-73px)] lg:w-64 lg:border-b-0 lg:border-r lg:px-4 lg:py-8">
      <nav className="flex gap-2 overflow-x-auto lg:flex-col">
        {navItems.map((item) => (
          <button
            key={item}
            onClick={() => setSection(item)}
            className={`flex shrink-0 items-center gap-3 rounded-xl px-4 py-3 text-left text-sm font-medium transition-colors ${
              section === item
                ? 'bg-slate-900 text-white'
                : 'text-gray-500 hover:bg-gray-100 hover:text-gray-900'
            }`}
          >
            {item === 'Pedir turno' ? (
                  <Search size={18} />
                ) : item.includes('turn') ? (
                  <CalendarDays size={18} />
                ) : (
                  <Users size={18} />
                )}
                {item}
          </button>
        ))}
      </nav>
    </aside>

    {/* ÁREA DE CONTENIDO PRINCIPAL */}
    <main className="min-w-0 flex-1 px-5 py-8 lg:px-12 lg:py-12">
  {/* Vista Cliente */}
  {user.rol === 'Cliente' && (
    <div className="space-y-8">
      {/* 1. SECCIÓN INICIO */}
      {section === 'Inicio' && (
        <>
          {/* Hero Banner */}
          <section className="hero-panel mb-8">
            <div className="relative z-10 flex max-w-xl flex-col gap-5">
              <div className="flex size-11 items-center justify-center rounded-xl bg-slate-900 text-white">
                <Sparkles />
              </div>
              <p className="text-sm font-semibold uppercase tracking-[0.18em] text-slate-800">
                Agenda simple, vida simple
              </p>
              <h1 className="text-balance text-4xl font-semibold tracking-tight text-gray-900 md:text-5xl">
                Encontrá un lugar para tu próximo turno.
              </h1>
              <p className="max-w-lg text-pretty text-base leading-relaxed text-gray-600">
                Reservá en segundos, recibí recordatorios y tené siempre tus turnos a mano.
              </p>
              <button
                onClick={() => setSection('Pedir turno')}
                className="mt-2 flex w-fit items-center gap-2 rounded-xl bg-slate-900 px-5 py-3 font-semibold text-white transition-transform hover:-translate-y-0.5"
              >
                Pedir un turno <ArrowRight size={18} />
              </button>
            </div>
            <div className="hero-mark" aria-hidden="true">
              <CalendarDays size={100} strokeWidth={1.2} />
            </div>
          </section>

          {/* Locales Recomendados */}
          <section>
            <p className="text-xs font-bold tracking-widest text-sky-600 uppercase mb-1">Para vos</p>
            <h2 className="text-2xl font-bold text-gray-900 mb-1">Locales recomendados</h2>
            <p className="text-sm text-gray-500 mb-6">Cerca tuyo y con disponibilidad esta semana.</p>
            <LocalesTable data={localFilter} user={user} />
          </section>
        </>
      )}

      {/* 2. SECCIÓN MIS TURNOS */}
      {section === 'Mis turnos' && (
        <section>
          <p className="text-xs font-bold tracking-widest text-sky-600 uppercase mb-1">Tu Agenda</p>
          <h2 className="text-3xl font-bold text-gray-900 mb-1">Mis turnos</h2>
          <p className="text-sm text-gray-500 mb-6">Todo lo que tenés reservado, en un solo lugar.</p>
          
          <TurnosTable idUsuario={user.id} />
        </section>
      )}

      {/* 3. SECCIÓN PEDIR TURNO / CATÁLOGO */}
      {section === 'Pedir turno' && (
        <section className="rounded-2xl border border-gray-200 bg-white p-6 shadow-sm">
          <h2 className="text-xl font-bold text-gray-900 mb-4">Catálogo de Locales</h2>
          <div className="mb-6 flex flex-col gap-4 sm:flex-row sm:items-center">
            <div className="flex-1">
              <SearchText value={searchText} onChange={(e) => setSearchText(e.target.value)} />
            </div>
            <div className="w-full sm:w-64">
              <Filter value={filterCategoria} onChange={(e) => setFilterCategoria(e.target.value)} />
            </div>
          </div>
          <LocalesTable data={localFilter} user={user} />
        </section>
      )}
    </div>
  )}

  {/* Vista Local */}
  {user.rol === 'Local' && (
    <section className="rounded-2xl border border-gray-200 bg-white p-6 shadow-sm">
      <h2 className="text-xl font-bold text-gray-900">Panel de Administración de tu Local</h2>
      <p className="mt-1 text-sm text-gray-500 mb-6">
        Acá se mostrará la agenda de turnos recibidos y la edición de horarios.
      </p>
      <LocalesTable data={localFilter} user={user} />
    </section>
  )}
</main>
    </div>
    </div>
  );
};
