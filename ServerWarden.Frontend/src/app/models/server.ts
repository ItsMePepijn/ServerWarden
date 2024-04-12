import { User } from "./auth";

export enum ServerType{
  ArkSurvivalEvolved,
}

export enum ServerPermissions
{
  None,
  SuperUser,
}

export interface ServerUser{
  user: User,
  permissions: ServerPermissions[],
}

export interface ServerProfileSimple{
  id: string,
  name: string,
  serverType: ServerType,
}

export interface ServerProfile extends ServerProfileSimple{
  installationPath: string,
  hasBeenInstalled: boolean,
  isInstalling: boolean,
  users: ServerUser[],
}