import { useCallback, useEffect } from "react";

const onKeyDown = (e) => {console.log(e.key)}

//const UseNavigationProps = ({isActive}) => {};


const useNavigation = ({isActive}) =>{
    useEffect(() => {
        if(isActive){
            document.addEventListener('keydown', onKeyDown)
        }
        return () => document.removeEventListener('keydown', onKeyDown)
    },[isActive])
    return 1
}

const onKeyDown = useCallback((e:KeyboardEvent) => {
    switch(e.key){
        case 'ArrowUp':
            decrement();
            break;
        case 'ArrowDown':
            increment();
            break;
        case 'ArrowLeft':
            decrement();
            break;
        case 'ArrowRight':
            increment();
        default:
            break;
    }
}, [])

export default useNavigation