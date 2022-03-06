#include "main.h"

#include <keychain/keychain.h>

keychain::Error* lastError = nullptr;

extern "C" {

EXPORTED bool setPassword(const char* package, const char* service, const char* user, const char* password)
{
	keychain::Error error{};
	keychain::setPassword(package, service, user, password, error);

	if (error)
	{
		lastError = &error;
		return false;
	}

	return true;
}

EXPORTED const char* getPassword(const char* package, const char* service, const char* user)
{
	keychain::Error error{};
	auto password = keychain::getPassword(package, service, user, error);

	if (error)
	{
		lastError = &error;
		return nullptr;
	}

	char* writable = new char[password.size() + 1];
	std::copy(password.begin(), password.end(), writable);
	writable[password.size()] = '\0';

	return writable;
}

EXPORTED bool deletePassword(const char* package, const char* service, const char* user)
{
	keychain::Error error{};
	keychain::deletePassword(package, service, user, error);

	if (error)
	{
		lastError = &error;
		return false;
	}

	return true;
}

EXPORTED const char* getLastErrorMessage()
{
	if (!lastError)
		return nullptr;

	char* writable = new char[lastError->message.size() + 1];
	std::copy(lastError->message.begin(), lastError->message.end(), writable);
	writable[lastError->message.size()] = '\0';

	return writable;
}

EXPORTED keychain::ErrorType getLastError()
{
	if (!lastError)
		return keychain::ErrorType::NoError;

	return lastError->type;
}
}
