
export interface ConfigUrl {
  siteType: ConfigUrlType,
  url: string
}

export enum ConfigUrlType {
  Payroll,
  Ess,
  Company
}