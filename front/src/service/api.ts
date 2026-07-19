import  {type LocalesType} from '../components/Locales/LocalesType.ts'

const API_URL = import.meta.env.VITE_API_URL;

export const getLocales = async (): Promise<LocalesType[]>  => {
    const rest = await fetch(`${API_URL}/api/Local`,{
        method: 'GET',
        headers: {
            'Content-type' : 'application/json',
        },
    });

    if(!rest.ok){
            const errorData = await rest.json().catch(() => ({}));
            throw new Error(errorData.message || 'Error al obtener los locales');
    }
    return rest.json();
};