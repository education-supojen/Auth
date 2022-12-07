VERSION:=V1.0.3
IMAGE_NAME:=supojen/education-auth:${VERSION}
COMPILE_DIRECTORY=$(if [!-d "./compile"])

publish:
	dotnet publish src/Auth.Presentation/Auth.Presentation.csproj -r linux-x64 --self-contained false --configuration Release -o compile 
	

push_image:
	docker build --rm -t ${IMAGE_NAME} . && docker push ${IMAGE_NAME}

release:
	git tag ${VERSION}
	git push origin ${VERSION}