# Exceptions

AlphaFS implements a few custom Exceptions:

Inherited from @System.IO.IOException:

    AlreadyExistsException
    DeviceNotReadyException
    DirectoryNotEmptyException
    NotAReparsePointException
    UnrecognizedReparsePointException


Inherited from @System.UnauthorizedAccessException:

    DirectoryReadOnlyException
    FileReadOnlyException


Inherited from @System.SystemException:

    InvalidTransactionException
    TransactionalConflictException
    TransactionAlreadyAbortedException
    TransactionAlreadyCommittedException
    UnsupportedRemoteTransactionException

**[TODO: Add links to the above classes]**