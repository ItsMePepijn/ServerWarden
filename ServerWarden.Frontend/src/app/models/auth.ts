export interface User{
  id: string
  name: string
}

export interface UserLogin{
  name: string
  password: string
}

export interface AuthState{
  user: User | null
  token: string | null
}