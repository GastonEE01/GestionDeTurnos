export interface UsuarioType {
  id: number;
  name: string;
  email: string;
  password: string;
  confirmPassword: string;
  rol: string;
}

export interface UsuarioRegisterDto {
  name: string;
  email: string;
  password: string;
  confirmPassword: string;
  rol: string;
}