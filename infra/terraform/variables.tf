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

variable "storage_account_container_name" {
  default = "poccontainer"
}

variable "container_name" {
  default = "container"
}

variable "state_container_name" {
  default = "container"
}
variable "state_key" {
  default = "key"
}



