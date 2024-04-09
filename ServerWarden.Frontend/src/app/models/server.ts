export enum ServerType{
  ArkSurvivalEvolved,
}

export enum ServerPermissions
{
  None,
  SuperUser,
}

export interface ServerProfile{
  id: string,
  name: string,
  serverType: ServerType,
  installationPath: string,
}