(Program
	(ProgramHeading
		Program 'program'
		(Identifier
			Letter 'F'
			Letter 'a'
			Letter 'c'
			Letter 't'
			Letter 'o'
			Letter 'r'
			Letter 'i'
			Letter 'a'
			Letter 'l'
		)
	)
	Semicolon ';'
	(ProgramBlock
		(Block
			(LabelDeclarationPart)
			(ConstantDefinitionPart)
			(TypeDefinitionPart)
			(VariableDeclarationPart
				Var 'var'
				(VariableDeclaration
					(IdentifierList
						(Identifier
							Letter 'n'
							Letter 'u'
							Letter 'm'
						)
						Comma ','
						(Identifier
							Letter 'f'
							Letter 'a'
							Letter 'c'
							Letter 't'
						)
						Comma ','
						(Identifier
							Letter 'i'
						)
					)
					Colon ':'
					(TypeDenoter
						(TypeIdentifier
							(Identifier
								Letter 'i'
								Letter 'n'
								Letter 't'
								Letter 'e'
								Letter 'g'
								Letter 'e'
								Letter 'r'
							)
						)
					)
				)
				Semicolon ';'
			)
			(ProcedureAndFunctionDeclarationPart)
			(StatementPart
				(CompoundStatement
					Begin 'begin'
					(StatementSequence
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(ActualParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter a number: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter a number: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WritelnParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter a number: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(Ambiguous ActualParameter
												(ActualParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'n'
																								Letter 'u'
																								Letter 'm'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'n'
																		Letter 'u'
																		Letter 'm'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(ProcedureIdentifier
														(Identifier
															Letter 'n'
															Letter 'u'
															Letter 'm'
														)
													)
												)
												(ActualParameter
													(FunctionIdentifier
														(Identifier
															Letter 'n'
															Letter 'u'
															Letter 'm'
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(ReadParameterList
											OpenParen '('
											(Ambiguous VariableAccess
												(VariableAccess
													(EntireVariable
														(VariableIdentifier
															(Identifier
																Letter 'n'
																Letter 'u'
																Letter 'm'
															)
														)
													)
												)
												(VariableAccess
													(ComponentVariable
														(FieldDesignator
															(FieldDesignatorIdentifier
																(Identifier
																	Letter 'n'
																	Letter 'u'
																	Letter 'm'
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(Ambiguous ReadlnParameterList
											(ReadlnParameterList
												OpenParen '('
												(FileVariable
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'n'
																		Letter 'u'
																		Letter 'm'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
											(ReadlnParameterList
												OpenParen '('
												(Ambiguous VariableAccess
													(VariableAccess
														(EntireVariable
															(VariableIdentifier
																(Identifier
																	Letter 'n'
																	Letter 'u'
																	Letter 'm'
																)
															)
														)
													)
													(VariableAccess
														(ComponentVariable
															(FieldDesignator
																(FieldDesignatorIdentifier
																	(Identifier
																		Letter 'n'
																		Letter 'u'
																		Letter 'm'
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Ambiguous Factor
																(Factor
																	(BoundIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																		)
																	)
																)
																(Factor
																	(Ambiguous VariableAccess
																		(VariableAccess
																			(EntireVariable
																				(VariableIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																					)
																				)
																			)
																		)
																		(VariableAccess
																			(ComponentVariable
																				(FieldDesignator
																					(FieldDesignatorIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(Factor
																	(UnsignedConstant
																		(ConstantIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																			)
																		)
																	)
																)
																(Factor
																	(FunctionDesignator
																		(FunctionIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(Ambiguous WritelnParameterList
											(WritelnParameterList
												OpenParen '('
												(FileVariable
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'n'
																		Letter 'u'
																		Letter 'm'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
											(WritelnParameterList
												OpenParen '('
												(WriteParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'n'
																								Letter 'u'
																								Letter 'm'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous AssignmentStatement
									(AssignmentStatement
										(Ambiguous VariableAccess
											(VariableAccess
												(EntireVariable
													(VariableIdentifier
														(Identifier
															Letter 'f'
															Letter 'a'
															Letter 'c'
															Letter 't'
														)
													)
												)
											)
											(VariableAccess
												(ComponentVariable
													(FieldDesignator
														(FieldDesignatorIdentifier
															(Identifier
																Letter 'f'
																Letter 'a'
																Letter 'c'
																Letter 't'
															)
														)
													)
												)
											)
										)
										Walrus ':='
										(Expression
											(SimpleExpression
												(Term
													(Factor
														(UnsignedConstant
															(UnsignedNumber
																(UnsignedInteger
																	(DigitSequence
																		Digit '1'
																	)
																)
															)
														)
													)
												)
											)
										)
									)
									(AssignmentStatement
										(FunctionIdentifier
											(Identifier
												Letter 'f'
												Letter 'a'
												Letter 'c'
												Letter 't'
											)
										)
										Walrus ':='
										(Expression
											(SimpleExpression
												(Term
													(Factor
														(UnsignedConstant
															(UnsignedNumber
																(UnsignedInteger
																	(DigitSequence
																		Digit '1'
																	)
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(RepetitiveStatement
									(ForStatement
										For 'for'
										(ControlVariable
											(EntireVariable
												(VariableIdentifier
													(Identifier
														Letter 'i'
													)
												)
											)
										)
										Walrus ':='
										(InitialValue
											(Expression
												(SimpleExpression
													(Term
														(Factor
															(UnsignedConstant
																(UnsignedNumber
																	(UnsignedInteger
																		(DigitSequence
																			Digit '1'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										To 'to'
										(FinalValue
											(Expression
												(SimpleExpression
													(Term
														(Ambiguous Factor
															(Factor
																(BoundIdentifier
																	(Identifier
																		Letter 'n'
																		Letter 'u'
																		Letter 'm'
																	)
																)
															)
															(Factor
																(Ambiguous VariableAccess
																	(VariableAccess
																		(EntireVariable
																			(VariableIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																				)
																			)
																		)
																	)
																	(VariableAccess
																		(ComponentVariable
																			(FieldDesignator
																				(FieldDesignatorIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																					)
																				)
																			)
																		)
																	)
																)
															)
															(Factor
																(UnsignedConstant
																	(ConstantIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																		)
																	)
																)
															)
															(Factor
																(FunctionDesignator
																	(FunctionIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Do 'do'
										(Statement
											(SimpleStatement
												(Ambiguous AssignmentStatement
													(AssignmentStatement
														(Ambiguous VariableAccess
															(VariableAccess
																(EntireVariable
																	(VariableIdentifier
																		(Identifier
																			Letter 'f'
																			Letter 'a'
																			Letter 'c'
																			Letter 't'
																		)
																	)
																)
															)
															(VariableAccess
																(ComponentVariable
																	(FieldDesignator
																		(FieldDesignatorIdentifier
																			(Identifier
																				Letter 'f'
																				Letter 'a'
																				Letter 'c'
																				Letter 't'
																			)
																		)
																	)
																)
															)
														)
														Walrus ':='
														(Expression
															(SimpleExpression
																(Term
																	(Ambiguous Factor
																		(Factor
																			(BoundIdentifier
																				(Identifier
																					Letter 'f'
																					Letter 'a'
																					Letter 'c'
																					Letter 't'
																				)
																			)
																		)
																		(Factor
																			(Ambiguous VariableAccess
																				(VariableAccess
																					(EntireVariable
																						(VariableIdentifier
																							(Identifier
																								Letter 'f'
																								Letter 'a'
																								Letter 'c'
																								Letter 't'
																							)
																						)
																					)
																				)
																				(VariableAccess
																					(ComponentVariable
																						(FieldDesignator
																							(FieldDesignatorIdentifier
																								(Identifier
																									Letter 'f'
																									Letter 'a'
																									Letter 'c'
																									Letter 't'
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																		(Factor
																			(UnsignedConstant
																				(ConstantIdentifier
																					(Identifier
																						Letter 'f'
																						Letter 'a'
																						Letter 'c'
																						Letter 't'
																					)
																				)
																			)
																		)
																		(Factor
																			(FunctionDesignator
																				(FunctionIdentifier
																					(Identifier
																						Letter 'f'
																						Letter 'a'
																						Letter 'c'
																						Letter 't'
																					)
																				)
																			)
																		)
																	)
																	(MultiplyingOperator
																		Asterisk '*'
																	)
																	(Ambiguous Factor
																		(Factor
																			(BoundIdentifier
																				(Identifier
																					Letter 'i'
																				)
																			)
																		)
																		(Factor
																			(Ambiguous VariableAccess
																				(VariableAccess
																					(EntireVariable
																						(VariableIdentifier
																							(Identifier
																								Letter 'i'
																							)
																						)
																					)
																				)
																				(VariableAccess
																					(ComponentVariable
																						(FieldDesignator
																							(FieldDesignatorIdentifier
																								(Identifier
																									Letter 'i'
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																		(Factor
																			(UnsignedConstant
																				(ConstantIdentifier
																					(Identifier
																						Letter 'i'
																					)
																				)
																			)
																		)
																		(Factor
																			(FunctionDesignator
																				(FunctionIdentifier
																					(Identifier
																						Letter 'i'
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
													)
													(AssignmentStatement
														(FunctionIdentifier
															(Identifier
																Letter 'f'
																Letter 'a'
																Letter 'c'
																Letter 't'
															)
														)
														Walrus ':='
														(Expression
															(SimpleExpression
																(Term
																	(Ambiguous Factor
																		(Factor
																			(BoundIdentifier
																				(Identifier
																					Letter 'f'
																					Letter 'a'
																					Letter 'c'
																					Letter 't'
																				)
																			)
																		)
																		(Factor
																			(Ambiguous VariableAccess
																				(VariableAccess
																					(EntireVariable
																						(VariableIdentifier
																							(Identifier
																								Letter 'f'
																								Letter 'a'
																								Letter 'c'
																								Letter 't'
																							)
																						)
																					)
																				)
																				(VariableAccess
																					(ComponentVariable
																						(FieldDesignator
																							(FieldDesignatorIdentifier
																								(Identifier
																									Letter 'f'
																									Letter 'a'
																									Letter 'c'
																									Letter 't'
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																		(Factor
																			(UnsignedConstant
																				(ConstantIdentifier
																					(Identifier
																						Letter 'f'
																						Letter 'a'
																						Letter 'c'
																						Letter 't'
																					)
																				)
																			)
																		)
																		(Factor
																			(FunctionDesignator
																				(FunctionIdentifier
																					(Identifier
																						Letter 'f'
																						Letter 'a'
																						Letter 'c'
																						Letter 't'
																					)
																				)
																			)
																		)
																	)
																	(MultiplyingOperator
																		Asterisk '*'
																	)
																	(Ambiguous Factor
																		(Factor
																			(BoundIdentifier
																				(Identifier
																					Letter 'i'
																				)
																			)
																		)
																		(Factor
																			(Ambiguous VariableAccess
																				(VariableAccess
																					(EntireVariable
																						(VariableIdentifier
																							(Identifier
																								Letter 'i'
																							)
																						)
																					)
																				)
																				(VariableAccess
																					(ComponentVariable
																						(FieldDesignator
																							(FieldDesignatorIdentifier
																								(Identifier
																									Letter 'i'
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																		(Factor
																			(UnsignedConstant
																				(ConstantIdentifier
																					(Identifier
																						Letter 'i'
																					)
																				)
																			)
																		)
																		(Factor
																			(FunctionDesignator
																				(FunctionIdentifier
																					(Identifier
																						Letter 'i'
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(ActualParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Factorial of ''
																)
															)
														)
													)
												)
											)
											Comma ','
											(Ambiguous ActualParameter
												(ActualParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'n'
																								Letter 'u'
																								Letter 'm'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 'n'
																					Letter 'u'
																					Letter 'm'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'n'
																		Letter 'u'
																		Letter 'm'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(ProcedureIdentifier
														(Identifier
															Letter 'n'
															Letter 'u'
															Letter 'm'
														)
													)
												)
												(ActualParameter
													(FunctionIdentifier
														(Identifier
															Letter 'n'
															Letter 'u'
															Letter 'm'
														)
													)
												)
											)
											Comma ','
											(ActualParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString '' is: ''
																)
															)
														)
													)
												)
											)
											Comma ','
											(Ambiguous ActualParameter
												(ActualParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 'f'
																				Letter 'a'
																				Letter 'c'
																				Letter 't'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'f'
																							Letter 'a'
																							Letter 'c'
																							Letter 't'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'f'
																								Letter 'a'
																								Letter 'c'
																								Letter 't'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 'f'
																					Letter 'a'
																					Letter 'c'
																					Letter 't'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 'f'
																					Letter 'a'
																					Letter 'c'
																					Letter 't'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'f'
																		Letter 'a'
																		Letter 'c'
																		Letter 't'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'f'
																			Letter 'a'
																			Letter 'c'
																			Letter 't'
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(ProcedureIdentifier
														(Identifier
															Letter 'f'
															Letter 'a'
															Letter 'c'
															Letter 't'
														)
													)
												)
												(ActualParameter
													(FunctionIdentifier
														(Identifier
															Letter 'f'
															Letter 'a'
															Letter 'c'
															Letter 't'
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Factorial of ''
																)
															)
														)
													)
												)
											)
											Comma ','
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Ambiguous Factor
																(Factor
																	(BoundIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																		)
																	)
																)
																(Factor
																	(Ambiguous VariableAccess
																		(VariableAccess
																			(EntireVariable
																				(VariableIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																					)
																				)
																			)
																		)
																		(VariableAccess
																			(ComponentVariable
																				(FieldDesignator
																					(FieldDesignatorIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(Factor
																	(UnsignedConstant
																		(ConstantIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																			)
																		)
																	)
																)
																(Factor
																	(FunctionDesignator
																		(FunctionIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											Comma ','
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString '' is: ''
																)
															)
														)
													)
												)
											)
											Comma ','
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Ambiguous Factor
																(Factor
																	(BoundIdentifier
																		(Identifier
																			Letter 'f'
																			Letter 'a'
																			Letter 'c'
																			Letter 't'
																		)
																	)
																)
																(Factor
																	(Ambiguous VariableAccess
																		(VariableAccess
																			(EntireVariable
																				(VariableIdentifier
																					(Identifier
																						Letter 'f'
																						Letter 'a'
																						Letter 'c'
																						Letter 't'
																					)
																				)
																			)
																		)
																		(VariableAccess
																			(ComponentVariable
																				(FieldDesignator
																					(FieldDesignatorIdentifier
																						(Identifier
																							Letter 'f'
																							Letter 'a'
																							Letter 'c'
																							Letter 't'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(Factor
																	(UnsignedConstant
																		(ConstantIdentifier
																			(Identifier
																				Letter 'f'
																				Letter 'a'
																				Letter 'c'
																				Letter 't'
																			)
																		)
																	)
																)
																(Factor
																	(FunctionDesignator
																		(FunctionIdentifier
																			(Identifier
																				Letter 'f'
																				Letter 'a'
																				Letter 'c'
																				Letter 't'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WritelnParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Factorial of ''
																)
															)
														)
													)
												)
											)
											Comma ','
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Ambiguous Factor
																(Factor
																	(BoundIdentifier
																		(Identifier
																			Letter 'n'
																			Letter 'u'
																			Letter 'm'
																		)
																	)
																)
																(Factor
																	(Ambiguous VariableAccess
																		(VariableAccess
																			(EntireVariable
																				(VariableIdentifier
																					(Identifier
																						Letter 'n'
																						Letter 'u'
																						Letter 'm'
																					)
																				)
																			)
																		)
																		(VariableAccess
																			(ComponentVariable
																				(FieldDesignator
																					(FieldDesignatorIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'u'
																							Letter 'm'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(Factor
																	(UnsignedConstant
																		(ConstantIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																			)
																		)
																	)
																)
																(Factor
																	(FunctionDesignator
																		(FunctionIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'u'
																				Letter 'm'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											Comma ','
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString '' is: ''
																)
															)
														)
													)
												)
											)
											Comma ','
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Ambiguous Factor
																(Factor
																	(BoundIdentifier
																		(Identifier
																			Letter 'f'
																			Letter 'a'
																			Letter 'c'
																			Letter 't'
																		)
																	)
																)
																(Factor
																	(Ambiguous VariableAccess
																		(VariableAccess
																			(EntireVariable
																				(VariableIdentifier
																					(Identifier
																						Letter 'f'
																						Letter 'a'
																						Letter 'c'
																						Letter 't'
																					)
																				)
																			)
																		)
																		(VariableAccess
																			(ComponentVariable
																				(FieldDesignator
																					(FieldDesignatorIdentifier
																						(Identifier
																							Letter 'f'
																							Letter 'a'
																							Letter 'c'
																							Letter 't'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(Factor
																	(UnsignedConstant
																		(ConstantIdentifier
																			(Identifier
																				Letter 'f'
																				Letter 'a'
																				Letter 'c'
																				Letter 't'
																			)
																		)
																	)
																)
																(Factor
																	(FunctionDesignator
																		(FunctionIdentifier
																			(Identifier
																				Letter 'f'
																				Letter 'a'
																				Letter 'c'
																				Letter 't'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(EmptyStatement)
							)
						)
					)
					End 'end'
				)
			)
		)
	)
	Dot '.'
)