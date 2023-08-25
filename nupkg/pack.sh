#!/bin/bash

version='0.0.12-dev'
source='http://nexus.codefr.com/repository/nuget-hosted/'
api_key='d5153e8a-6074-308d-85cc-c7cf932a9e57'

pack(){
  echo -e "\n============ Build Module $1 ============\n"
  projects=($@)
  len=${#projects[@]}
  for ((i=1; i<$len; i++)); do
    project=${projects[i]}
    project_url="../src/${project}"
    
    dotnet restore $project_url
    echo "project ${project} restored!"
  
    dotnet pack $project_url -c Release -o ./
    echo "project ${project} packaged!"
  
    package_name="${project}.${version}.nupkg"
    dotnet nuget push $package_name -s $source --api-key $api_key
    echo "package ${package_name} pushed!"

    rm -rf $package_name
    echo "package ${package_name} deleted!"
  done
}



pack_projects=('LinkMore.KA.AM.Domain.Shared' 'LinkMore.KA.AM.Application.Contracts' 'LinkMore.KA.AM.Sdk')

pack 'AM Sdk' ${pack_projects[@]}
