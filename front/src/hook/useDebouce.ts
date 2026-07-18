import { useEffect, useState } from "react";

export function useDebouce(value: string,delay: number){
    const [debouceValue, setDebouceTime] = useState(value)

    useEffect(() => {
        const timer = setTimeout(() => {
            setDebouceTime(value)
        },delay)
        return () => {
            clearTimeout(timer)
        }
        },[value,delay])
        return debouceValue
}