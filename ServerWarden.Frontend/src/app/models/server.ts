export enum ServerType{
  ArkSurvivalEvolved,
}

export enum ServerPermissions
{
  None,
  SuperUser,
}

export interface ServerProfileSimple{
  id: string,
  name: string,
  serverType: ServerType,
}