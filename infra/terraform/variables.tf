variable "resource_group_name" {
  default = "poc-rg"
}

variable "state_resource_group_name" {
  default = "poc-rg"
}

variable "location" {
  default = "West Europe"
}

# Web app
variable "funcapp_name" {
  default = "poc-webapp"
}

variable "storage_account_name" {
  default = "pocstorageaccount"
}

variable "state_storage_account_name" {
  default = "pocstorageaccount"
}
variable "state_container_name" {
  default = "container"
}
variable "state_key" {
  default = "key"
}

variable "subscription_id" {
  description = "The Azure subscription ID."
}
variable "client_id" {
  description = "The Azure Service Principal app ID."
}
variable "client_secret" {
  description = "The Azure Service Principal password."
}

variable "tenant_id" {
  description = "The Azure Tenant ID."
}