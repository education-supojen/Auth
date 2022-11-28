version:=1.0.2
IMAGE_NAME:=supojen/education-auth:${version}

compile:
	dotnet publish src/Auth.Presentation/Auth.Presentation.csproj -r linux-x64 --self-contained false --configuration Release -o compile

print_version: 
	echo ${version}

print_image:
	echo ${IMAGE_NAME}

publish:
	docker build --rm -t ${IMAGE_NAME} . && docker push ${IMAGE_NAME}