VERSION:=v1.0.4
IMAGE_NAME:=supojen/education-auth:${VERSION}

version:
	${VERSION}
	
push_image:
	docker build --rm -t ${IMAGE_NAME} . && docker push ${IMAGE_NAME}

release:
	git tag ${VERSION}
	git push origin ${VERSION}