VERSION:=1.1.0
IMAGE_NAME:=supojen/education-auth:${version}

compile:
	dotnet publish src/Auth.Presentation/Auth.Presentation.csproj -r linux-x64 --self-contained false --configuration Release -o compile

version: 
	echo ${VERSION}

image:
	echo ${IMAGE_NAME}:${version}

publish:
	docker build --rm -t ${IMAGE_NAME} . && docker push ${IMAGE_NAME}